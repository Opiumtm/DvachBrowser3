using System;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство рендеринга Direct2D.
    /// </summary>
    public sealed class Direct2DTextRender2Renderer : ITextRender2VirtualRenderer
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        public Direct2DTextRender2Renderer(ITextRender2RenderCallback callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            Callback = callback;
        }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        public ITextRender2RenderCallback Callback { get; }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        /// <param name="map">Карта расположений.</param>
        /// <param name="session">Сессия.</param>
        /// <param name="region">Регион.</param>
        /// <returns>Результат.</returns>
        public void Render(ITextRender2MeasureMap map, CanvasDrawingSession session, Rect region)
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
                foreach (var element in line.GetMeasureMap())
                {
                    var drawRect = new Rect(element.Placement, element.Size);
                    var testRect = drawRect;
                    testRect.Intersect(region);
                    if (!testRect.IsEmpty)
                    {
                        DrawElement(element, session, region);
                    }
                }
            }
        }

        private void DrawElement(TextRender2MeasureMapElement element, CanvasDrawingSession session, Rect region)
        {
            var command = element.Command;
            string text;
            var textCnt = command.Content as ITextRenderTextContent;
            if (textCnt != null)
            {
                text = textCnt.Text ?? "";
            }
            else
            {
                text = "";
            }
            using (var format = new CanvasTextFormat())
            {
                format.FontFamily = "Segoe UI";
                format.FontSize = (float)Callback.PostFontSize;
                format.WordWrapping = CanvasWordWrapping.NoWrap;
                format.TrimmingGranularity = CanvasTextTrimmingGranularity.None;
                format.HorizontalAlignment = CanvasHorizontalAlignment.Left;
                session.DrawText(text, new Vector2((float)element.Placement.X, (float)element.Placement.Y), Colors.Black, format);
            }
        }
    }
}