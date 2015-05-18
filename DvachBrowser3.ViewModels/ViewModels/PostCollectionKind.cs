namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тип коллекции постов.
    /// </summary>
    public enum PostCollectionKind
    {
        /// <summary>
        /// Тред.
        /// </summary>
        Thread,

        /// <summary>
        /// Предварительный просмотр треда.
        /// </summary>
        ThreadPreview,

        /// <summary>
        /// Архивный тред.
        /// </summary>
        Archive,

        /// <summary>
        /// Другой тип.
        /// </summary>
        Other,
    }
}