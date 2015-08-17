namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Команда рендеринга.
    /// </summary>
    public interface ITextRenderCommand
    {
        /// <summary>
        /// Атрибуты.
        /// </summary>
        ITextRenderAttributeState Attributes { get; }

        /// <summary>
        /// Контент.
        /// </summary>
        ITextRenderContent Content { get; }
    }
}