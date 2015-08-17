namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Элемент программы печати текста.
    /// </summary>
    public sealed class TextRenderProgramElement : ITextRenderProgramElement
    {
        public TextRenderProgramElement(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id
        {
            get { return CommonRenderProgramElements.PrintText; }
        }

        /// <summary>
        /// Текст.
        /// </summary>
        public string Text { get; private set; }
    }
}