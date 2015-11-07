namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Ссылка.
    /// </summary>
    public interface ITextRenderLinkAttribute : ITextRenderAttribute
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        string Uri { get; } 

        /// <summary>
        /// Дополнительные данные.
        /// </summary>
        object CustomData { get; }
    }
}