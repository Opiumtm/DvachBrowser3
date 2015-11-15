namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим работы загрузчика страницы.
    /// </summary>
    public enum BoardPageLoaderUpdateMode
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
    }
}