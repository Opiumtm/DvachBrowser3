using System.Collections.Generic;
using Windows.Foundation;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Карта размещения элементов.
    /// </summary>
    public interface ITextRender2MeasureMap
    {
        /// <summary>
        /// Получить карту размещения.
        /// </summary>
        /// <returns>Карта размещения.</returns>
        IReadOnlyList<ITextRender2MeasureMapLine> GetMeasureMapLines();

        /// <summary>
        /// Максимальное число линий.
        /// </summary>
        int? MaxLines { get; }

        /// <summary>
        /// Превышено число линий.
        /// </summary>
        bool ExceedLines { get; }

        /// <summary>
        /// Размеры.
        /// </summary>
        Size Bounds { get; }

        /// <summary>
        /// Коэффициент для рендеринга зачёркнутого текста.
        /// </summary>
        double StrikethrougKoef { get; }
    }
}