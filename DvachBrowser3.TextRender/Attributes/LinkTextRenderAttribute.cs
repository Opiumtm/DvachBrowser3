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
        /// <param name="data">Данные.</param>
        public LinkTextRenderAttribute(string uri, object data = null)
        {
            Uri = uri;
            CustomData = data;
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

        /// <summary>
        /// Дополнительные данные.
        /// </summary>
        public object CustomData { get; private set; }
    }
}