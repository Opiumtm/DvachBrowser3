using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.TextRender
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class TextRender2Control : UserControl
    {
        public TextRender2Control()
        {
            InitializeComponent();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            bool isChanged;
            var result = DoMeasureOverride(availableSize, out isChanged);
            if (isChanged)
            {
                TriggerRender();
            }
            return result;
        }

        private Size DoMeasureOverride(Size availableSize, out bool isChanged)
        {
            SetCachedMap(availableSize.Width, out isChanged);
            if (cachedMap == null)
            {
                return base.MeasureOverride(availableSize);
            }
            return new Size(cachedMap.Bounds.Width, cachedMap.Bounds.Height);
        }

        private void TriggerRender()
        {
            MainGrid.Visibility = Visibility.Collapsed;
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, RenderText);
        }

        private void RenderText()
        {
            try
            {
                var map = cachedMap;
                if (lastMapId != mapId && map != null)
                {
                    lastMapId = mapId;
                    var renderer = new XamlCanvasTextRender2Renderer(ControlCallback);
                    var textRender = renderer.Render(map);
                    textRender.HorizontalAlignment = HorizontalAlignment.Left;
                    textRender.VerticalAlignment = VerticalAlignment.Top;
                    MainGrid.Children.Clear();
                    MainGrid.Children.Add(textRender);
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                MainGrid.Visibility = Visibility.Visible;
            }
        }

        private void SetCachedMap(double width, out bool isChanged)
        {
            isChanged = false;
            var program = ControlCallback?.GetRenderProgram();
            if (double.IsNaN(width) || double.IsInfinity(width) || width < 50 || ControlCallback == null || program == null)
            {
                cachedMap = null;
                MainGrid.Children.Clear();
                return;
            }
            bool needRedraw = false;
            if (cachedMap == null)
            {
                needRedraw = true;
            }
            else if (lastWidth != width)
            {
                needRedraw = true;
            }
            else if (ControlCallback.PostFontSize != lastFontSize)
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
                cachedMap = DoMeasure(width, program);
                mapId = Guid.NewGuid();
                ExceedLines = cachedMap.ExceedLines;
                isChanged = true;
            }
        }

        private ITextRender2MeasureMap DoMeasure(double width, ITextRender2RenderProgram program)
        {
            var mapper = new Direct2DTextRender2MeasureMapper();
            return mapper.CreateMap(program, width, ControlCallback.PostFontSize, MaxLines > 0 ? (int?)MaxLines : null);
        }

        private double? lastWidth;
        private double? lastFontSize;
        private int? lastMaxLines;
        private ITextRender2MeasureMap cachedMap;
        private Guid? lastId;
        private Guid mapId;
        private Guid? lastMapId;

        public void InvalidateRenderedState()
        {
            InvalidateMeasure();
        }

        public void InvalidateRenderedStateAndClear()
        {
            cachedMap = null;
            InvalidateMeasure();
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
        public static readonly DependencyProperty ExceedLinesProperty = DependencyProperty.Register("ExceedLines", typeof (bool), typeof (TextRender2Control), new PropertyMetadata(false, ExceedLinesPropertyChangedCallback));

        private static void ExceedLinesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TextRender2Control;
            obj?.InvokeTextRendered();
        }

        private void InvokeTextRendered()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextRendered?.Invoke(this, EventArgs.Empty);
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
        public static readonly DependencyProperty ControlCallbackProperty = DependencyProperty.Register("ControlCallback", typeof (ITextRender2ControlCallback), typeof (TextRender2Control), new PropertyMetadata(null, (d, e) => (d as TextRender2Control)?.InvalidateRenderedStateAndClear()));
    }
}
