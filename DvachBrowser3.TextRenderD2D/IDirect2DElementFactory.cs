using System.Numerics;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Фабрика элементов Direct 2D.
    /// </summary>
    public interface IDirect2DElementFactory
    {
        /// <summary>
        /// Вычислить размеры команды.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Размеры.</returns>
        Size MeasureCommand(ITextRenderCommand command);

        /// <summary>
        /// Нарисовать.
        /// </summary>
        /// <param name="drawAt">Где нарисовать.</param>
        /// <param name="session">Сессия.</param>
        /// <param name="command">Команда.</param>
        /// <returns>Размеры.</returns>
        Size DrawCommand(Point drawAt, CanvasDrawingSession session, ITextRenderCommand command);

        /// <summary>
        /// Получить ключ кэша.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Ключ кэша.</returns>
        string GetCacheKey(ITextRenderCommand command);
    }
}