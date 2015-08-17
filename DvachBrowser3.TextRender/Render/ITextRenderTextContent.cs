namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Текстовый контент.
    /// </summary>
    public interface ITextRenderTextContent : ITextRenderContent
    {
        /// <summary>
        /// Текст.
        /// </summary>
        string Text { get; } 
    }
}