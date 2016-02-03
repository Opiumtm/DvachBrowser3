using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.NetworkOperators;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.TextRender
{
    public sealed partial class TextRender2Control : UserControl
    {
        private readonly SizeChangeDelayHelper sizeChangeDelay;

        public TextRender2Control()
        {
            this.InitializeComponent();
            sizeChangeDelay = new SizeChangeDelayHelper(TimeSpan.FromSeconds(0.25), Dispatcher);
            sizeChangeDelay.SizeUpdated += (sender, e) =>
            {
                InvalidateRenderedState();
            };
            MainGrid.SizeChanged += sizeChangeDelay.OnSizeChanged;
            InvalidateRenderedState();
            this.Loaded += (sender, e) =>
            {
                InvalidateRenderedState();
            };
        }

        private double? lastWidth;
        private ITextRender2RenderProgram lastProgram;
        private double? lastFontSize;
        private int? lastMaxLines;
        private ITextRender2MeasureMap cachedMap;

        public void InvalidateRenderedState()
        {
            var availableSize = new Size(MainGrid.ActualWidth, MainGrid.ActualHeight);
            MainGrid.Children.Clear();
            if (ControlCallback == null || availableSize.Width < 50 || double.IsNaN(availableSize.Width) || double.IsInfinity(availableSize.Width))
            {
                return;
            }
            var program = ControlCallback.GetRenderProgram();
            if (program == null)
            {
                return;
            }
            var mapper = new Direct2DTextRender2MeasureMapper();

            if (cachedMap == null || lastWidth != availableSize.Width || lastProgram != program || lastFontSize != ControlCallback.PostFontSize || lastMaxLines != MaxLines)
            {
                cachedMap = mapper.CreateMap(program, availableSize.Width, ControlCallback.PostFontSize, MaxLines > 0 ? (int?)MaxLines : null);
                lastWidth = availableSize.Width;
                lastProgram = program;
                lastFontSize = ControlCallback.PostFontSize;
                lastMaxLines = MaxLines;
            }

            var map = cachedMap;
            var renderer = new XamlCanvasTextRender2Renderer(ControlCallback);
            var textRender = renderer.Render(map);
            textRender.HorizontalAlignment = HorizontalAlignment.Left;
            textRender.VerticalAlignment = VerticalAlignment.Top;
            MainGrid.Children.Add(textRender);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ExceedLines = map.ExceedLines;
                TextRendered?.Invoke(this, EventArgs.Empty);
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public event EventHandler TextRendered;

        /// <summary>
        /// Превышено количество линий.
        /// </summary>
        public bool ExceedLines
        {
            get { return (bool) GetValue(ExceedLinesProperty); }
            set { SetValue(ExceedLinesProperty, value); }
        }

        /// <summary>
        /// Превышено количество линий.
        /// </summary>
        public static readonly DependencyProperty ExceedLinesProperty = DependencyProperty.Register("ExceedLines", typeof (bool), typeof (TextRender2Control), new PropertyMetadata(false));

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public int MaxLines
        {
            get { return (int) GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof (int), typeof (TextRender2Control), new PropertyMetadata(0, (d, e) => (d as TextRender2Control)?.InvalidateRenderedState()));

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        public ITextRender2ControlCallback ControlCallback
        {
            get { return (ITextRender2ControlCallback) GetValue(ControlCallbackProperty); }
            set { SetValue(ControlCallbackProperty, value); }
        }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        public static readonly DependencyProperty ControlCallbackProperty = DependencyProperty.Register("ControlCallback", typeof (ITextRender2ControlCallback), typeof (TextRender2Control), new PropertyMetadata(null, (d, e) => (d as TextRender2Control)?.InvalidateRenderedState()));

        /// <summary>
        /// Помощник задержки изменения размеров.
        /// </summary>
        private sealed class SizeChangeDelayHelper
        {
            private readonly CoreDispatcher dispatcher;

            /// <summary>
            /// Конструктор.
            /// </summary>
            /// <param name="sizeChangeDelay">Задержка.</param>
            public SizeChangeDelayHelper(TimeSpan sizeChangeDelay, CoreDispatcher dispatcher)
            {
                this.sizeChangeDelay = sizeChangeDelay;
                this.dispatcher = dispatcher;
            }

            /// <summary>
            /// Размер изменён.
            /// </summary>
            public event EventHandler SizeUpdated;

            private readonly TimeSpan sizeChangeDelay;

            private int isFirst;

            /// <summary>
            /// Событие по изменению.
            /// </summary>
            /// <param name="sender">Источник события.</param>
            /// <param name="e">Событие.</param>
            public void OnSizeChanged(object sender, SizeChangedEventArgs e)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (Interlocked.Exchange(ref isFirst, 1) == 0)
                {
                    dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        try
                        {
                            await OnSizeUpdated();
                        }
                        catch (Exception ex)
                        {
                            // ignore
                        }
                    });
                    return;
                }
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        await DelayAction();
                    }
                    catch (Exception ex)
                    {
                        // ignore
                    }
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            private bool isWaiting;

            private bool isRefreshed;

            private DateTime stamp;

            private async Task DelayAction()
            {
                stamp = DateTime.Now;
                isRefreshed = true;
                if (isWaiting)
                {
                    return;
                }
                isWaiting = true;
                try
                {
                    while (isRefreshed)
                    {
                        isRefreshed = false;
                        var toWait = (stamp.Add(sizeChangeDelay) - DateTime.Now);
                        if (toWait.Ticks > 0)
                        {
                            await Task.Delay(toWait);
                        }
                    }
                }
                finally
                {
                    isWaiting = false;
                }
                OnSizeUpdated();
            }

            private Task OnSizeUpdated()
            {
                SizeUpdated?.Invoke(this, EventArgs.Empty);
                return Task.CompletedTask;
            }
        }
    }
}
