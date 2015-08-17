namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Элемент программы с текстом.
    /// </summary>
    public interface ITextRenderProgramElement : IRenderProgramElement
    {
        /// <summary>
        /// Текст.
        /// </summary>
        string Text { get; } 
    }
}