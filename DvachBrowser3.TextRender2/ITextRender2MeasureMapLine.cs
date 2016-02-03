using System.Collections.Generic;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Линия.
    /// </summary>
    public interface ITextRender2MeasureMapLine
    {
        /// <summary>
        /// Получить карту размещения.
        /// </summary>
        /// <returns>Карта размещения.</returns>
        IReadOnlyList<TextRender2MeasureMapElement> GetMeasureMap();

        /// <summary>
        /// Номер строки (с нулевой).
        /// </summary>
        int LineNumber { get; }

        /// <summary>
        /// Высота.
        /// </summary>
        double Height { get; }
    }
}