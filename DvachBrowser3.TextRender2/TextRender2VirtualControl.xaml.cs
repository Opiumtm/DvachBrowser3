using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.TextRender
{
    public sealed partial class TextRender2VirtualControl : UserControl
    {
        public TextRender2VirtualControl()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            VirtualCanvas.RegionsInvalidated -= VirtualCanvasOnRegionsInvalidated;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            VirtualCanvas.RegionsInvalidated += VirtualCanvasOnRegionsInvalidated;
        }

        public async void InvalidateRenderedState()
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    cachedMap = null;
                    InvalidateMeasure();
                });
            }
            catch
            {
            }
        }

        private ITextRender2MeasureMap cachedMap;

        private double lastWidth;
        private double lastFontSize;
        private int? lastMaxLines;
        private Guid? lastId;

        protected override Size MeasureOverride(Size availableSize)
        {
            SetCachedMap(availableSize.Width);
            if (cachedMap == null)
            {
                return base.MeasureOverride(availableSize);
            }
            return new Size(cachedMap.Bounds.Width, cachedMap.Bounds.Height);
        }

        private void SetCachedMap(double width)
        {
            if (Math.Abs(VirtualCanvas.Width - width) > 0.01)
            {
                VirtualCanvas.Width = width;
            }
            if (double.IsNaN(width) || double.IsInfinity(width) || width < 50 || ControlCallback == null)
            {
                cachedMap = null;
                VirtualCanvas.Height = 1;
                return;
            }
            bool needRedraw = false;
            if (cachedMap == null)
            {
                needRedraw = true;
            }
            else if (Math.Abs(lastWidth - width) > 0.01)
            {
                needRedraw = true;
            }
            else if (Math.Abs(ControlCallback.PostFontSize - lastFontSize) > 0.01)
            {
                needRedraw = true;
            }
            else if (lastMaxLines != MaxLines)
            {
                needRedraw = true;
            }
            else if (lastId != ControlCallback.ProgramId)
            {
                needRedraw = true;
            }
            lastWidth = width;
            lastFontSize = ControlCallback.PostFontSize;
            lastMaxLines = MaxLines;
            lastId = ControlCallback.ProgramId;
            if (needRedraw)
            {
                cachedMap = DoMeasure(width);
                VirtualCanvas.Height = cachedMap.Bounds.Height;
            }
        }

        private ITextRender2MeasureMap DoMeasure(double width)
        {
            var mapper = new Direct2DTextRender2MeasureMapper();
            return mapper.CreateMap(ControlCallback.GetRenderProgram(), width, ControlCallback.PostFontSize, MaxLines > 0 ? (int?) MaxLines : null);
        }

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
        public static readonly DependencyProperty ControlCallbackProperty = DependencyProperty.Register("ControlCallback", typeof (ITextRender2ControlCallback), typeof (TextRender2VirtualControl), new PropertyMetadata(null, ControlCallbackPropertyChangedCallback));

        private static void ControlCallbackPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TextRender2VirtualControl;
            obj?.InvalidateMeasure();
        }

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
        public static readonly DependencyProperty ExceedLinesProperty = DependencyProperty.Register("ExceedLines", typeof (bool), typeof (TextRender2VirtualControl), new PropertyMetadata(false, ExceedLinesPropertyChangedCallback));

        private static void ExceedLinesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TextRender2VirtualControl;
            obj?.InvokeTextRendered();
        }

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
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof (int), typeof (TextRender2VirtualControl), new PropertyMetadata(0, MaxLinesPropertyChangedCallback));

        private static void MaxLinesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TextRender2VirtualControl;
            obj?.InvalidateMeasure();
        }

        public event EventHandler TextRendered;

        private void InvokeTextRendered()
        {
            TextRendered?.Invoke(this, EventArgs.Empty);
        }

        private void VirtualCanvasOnRegionsInvalidated(CanvasVirtualControl sender, CanvasRegionsInvalidatedEventArgs args)
        {
            if (cachedMap != null && ControlCallback != null)
            {
                ITextRender2VirtualRenderer renderer = new Direct2DTextRender2Renderer(ControlCallback);
                foreach (var r in args.InvalidatedRegions)
                {
                    using (var session = sender.CreateDrawingSession(r))
                    {
                        session.DrawRectangle(r, Colors.Black);
                        renderer.Render(cachedMap, session, r);
                    }
                }
            }
        }
    }
}
