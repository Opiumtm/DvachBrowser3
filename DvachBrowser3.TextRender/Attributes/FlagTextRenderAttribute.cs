namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Стандартные атрибуты.
    /// </summary>
    public sealed class FlagTextRenderAttribute : ITextRenderAttribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        public FlagTextRenderAttribute(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор атрибута.
        /// </summary>
        public string Id { get; private set; }
    }
}