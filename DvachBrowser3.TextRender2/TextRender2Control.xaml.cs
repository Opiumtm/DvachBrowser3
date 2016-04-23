using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DvachBrowser3_TextRender_Native;

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

        private Guid renderId = Guid.Empty;
        private Guid renderedId = Guid.Empty;

        protected override Size MeasureOverride(Size availableSize)
        {
            bool isChanged;
            var result = DoMeasureOverride(availableSize, out isChanged);
            if (isChanged)
            {
                renderId = Guid.NewGuid();
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
            //DispatchRenderText(Dispatcher, this);
            return new Size(cachedMap.Bounds.Width, cachedMap.Bounds.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            RenderText();
            return base.ArrangeOverride(finalSize);
        }

        private void RenderText()
        {
            if (renderedId != renderId)
            {
                renderedId = renderId;
                DoRenderText();
            }
        }

        private static readonly MassChildUpdateHelper ChildUpdateHelper = new MassChildUpdateHelper();

        private void DoRenderText()
        {
            if (RenderSuspended)
            {
                return;
            }
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
                    var a = new UIElement[1];
                    a[0] = textRender;
                    ChildUpdateHelper.UpdateChildren(MainGrid.Children, a);
                }
            }
            catch
            {
                // ignored
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

        private void RenderSuspendedChanged()
        {
            if (RenderSuspended)
            {
                MainGrid.Children.Clear();
            }
            else
            {
                try
                {
                    mapId = Guid.NewGuid();
                    DoRenderText();
                }
                catch
                {
                    // Ignore
                }
            }
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
            TextRendered?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Отображение прекращено.
        /// </summary>
        public bool RenderSuspended
        {
            get { return (bool) GetValue(RenderSuspendedProperty); }
            set { SetValue(RenderSuspendedProperty, value); }
        }

        /// <summary>
        /// Отображение прекращено.
        /// </summary>
        public static readonly DependencyProperty RenderSuspendedProperty = DependencyProperty.Register("RenderSuspended", typeof (bool), typeof (TextRender2Control), new PropertyMetadata(false, RenderSuspendedPropertyChangedCallback));

        private static void RenderSuspendedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextRender2Control)d).RenderSuspendedChanged();
        }
    }
}
