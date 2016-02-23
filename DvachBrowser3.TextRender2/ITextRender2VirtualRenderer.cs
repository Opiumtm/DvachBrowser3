using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство виртуального рендеринга разметки.
    /// </summary>
    public interface ITextRender2VirtualRenderer
    {
        /// <summary>
        /// Обратный вызов.
        /// </summary>
        ITextRender2RenderCallback Callback { get; }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        /// <param name="map">Карта расположений.</param>
        /// <param name="session">Сессия.</param>
        /// <param name="region">Регион.</param>
        /// <returns>Результат.</returns>
        void Render(ITextRender2MeasureMap map, CanvasDrawingSession session, Rect region);
    }
}