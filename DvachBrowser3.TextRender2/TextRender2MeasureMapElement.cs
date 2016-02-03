using Windows.Foundation;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Элемент измерения.
    /// </summary>
    public struct TextRender2MeasureMapElement
    {
        /// <summary>
        /// Команда.
        /// </summary>
        public ITextRenderCommand Command;

        /// <summary>
        /// Положение.
        /// </summary>
        public Point Placement;

        /// <summary>
        /// Размеры.
        /// </summary>
        public Size Size;
    }
}