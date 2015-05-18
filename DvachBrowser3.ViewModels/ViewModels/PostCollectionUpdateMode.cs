namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Режим обновления коллекции постов.
    /// </summary>
    public enum PostCollectionUpdateMode
    {
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
    }
}