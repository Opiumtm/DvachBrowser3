namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Ссылка.
    /// </summary>
    public sealed class LinkTextRenderAttribute : ITextRenderLinkAttribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="uri">Ссылка.</param>
        public LinkTextRenderAttribute(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id
        {
            get { return CommonTextRenderAttributes.Link; }
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public string Uri { get; private set; }
    }
}