namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Перевод строки.
    /// </summary>
    public sealed class TextRenderLineBreakContent : ITextRenderContent
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id
        {
            get { return CommonTextRenderContent.LineBreak; }
        }
    }
}