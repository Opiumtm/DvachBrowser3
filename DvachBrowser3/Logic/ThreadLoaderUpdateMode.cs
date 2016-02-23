namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим работы загрузчика треда.
    /// </summary>
    public enum ThreadLoaderUpdateMode
    {
        /// <summary>
        /// Загрузить.
        /// </summary>
        Load,

        /// <summary>
        /// Получить из кэша.
        /// </summary>
        GetFromCache,

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        CheckForUpdates,

        /// <summary>
        /// Полная загрузка.
        /// </summary>
        LoadFull,

        /// <summary>
        /// Полная пересинхронизация.
        /// </summary>
        ResyncFull,

        /// <summary>
        /// Получить из кэша в режиме оффлайн.
        /// </summary>
        GetFromCacheOffline
    }
}