namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Режим обновления коллекции постов.
    /// </summary>
    public enum PostCollectionUpdateMode
    {
        /// <summary>
        /// По умолчанию.
        /// </summary>
        Default,

        /// <summary>
        /// Частичное обновление.
        /// </summary>
        Partial,

        /// <summary>
        /// Полное обновление.
        /// </summary>
        Full,

        /// <summary>
        /// Полная перезагрузка треда.
        /// </summary>
        Reload,

        /// <summary>
        /// Загружено из кэша.
        /// </summary>
        FromCache,
    }
}