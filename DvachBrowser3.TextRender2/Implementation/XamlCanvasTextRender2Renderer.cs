using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DvachBrowser3_TextRender_Native;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство рендеринга разметки.
    /// </summary>
    public sealed class XamlCanvasTextRender2Renderer : ITextRender2Renderer
    {
        private static readonly MassChildUpdateHelper UpdateHelper = new MassChildUpdateHelper();

        private static readonly XamlRenderHelper RenderHelper = new XamlRenderHelper();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        public XamlCanvasTextRender2Renderer(ITextRender2RenderCallback callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            Callback = new ColorsCacheCallback(callback);
        }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        public ITextRender2RenderCallback Callback { get; }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        /// <param name="map">Карта расположений.</param>
        /// <returns>Результат.</returns>
        public FrameworkElement Render(ITextRender2MeasureMap map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            var canvas = new Canvas()
            {
                Height = map.Bounds.Height,
                Width = map.Bounds.Width
            };
            DoRender(canvas, map);
            return canvas;
        }

        private void DoRender(Canvas canvas, ITextRender2MeasureMap map)
        {
            var elements = DoRenderLines(map).ToArray();
            UpdateHelper.UpdateChildren(canvas.Children, elements);
        }

        private IEnumerable<UIElement> DoRenderLines(ITextRender2MeasureMap map)
        {
            foreach (var line in map.GetMeasureMapLines())
            {
                if (map.MaxLines.HasValue)
                {
                    if (line.LineNumber >= map.MaxLines.Value)
                    {
                        break;
                    }
                }
                foreach (var el in DoRenderLine(map, line))
                {
                    yield return el;
                }
            }
        }

        private IEnumerable<UIElement> DoRenderLine(ITextRender2MeasureMap map, ITextRender2MeasureMapLine line)
        {
            foreach (var el in line.GetMeasureMap())
            {
                yield return DoRenderElement(map, line, el);
            }
        }

        private UIElement DoRenderElement(ITextRender2MeasureMap map, ITextRender2MeasureMapLine line, TextRender2MeasureMapElement el)
        {
            return RenderHelper.RenderElement(new NativeRenderArgument(map, line, el, Callback));
        }
    }
}