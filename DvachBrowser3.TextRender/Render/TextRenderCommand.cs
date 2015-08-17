namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Команда рендеринга.
    /// </summary>
    public sealed class TextRenderCommand : ITextRenderCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="attributes">Атрибуты.</param>
        /// <param name="content">Контент.</param>
        public TextRenderCommand(ITextRenderAttributeState attributes, ITextRenderContent content)
        {
            Attributes = attributes;
            Content = content;
        }

        /// <summary>
        /// Атрибуты.
        /// </summary>
        public ITextRenderAttributeState Attributes { get; private set; }

        /// <summary>
        /// Контент.
        /// </summary>
        public ITextRenderContent Content { get; private set; }
    }
}