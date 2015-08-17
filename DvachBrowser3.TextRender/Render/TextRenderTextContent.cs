namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Текстовый контент.
    /// </summary>
    public sealed class TextRenderTextContent : ITextRenderTextContent
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="text">Текст.</param>
        public TextRenderTextContent(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id
        {
            get { return CommonTextRenderContent.Text; }
        }

        /// <summary>
        /// Текст.
        /// </summary>
        public string Text { get; private set; }
    }
}