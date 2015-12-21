namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим работы загрузчика каталога.
    /// </summary>
    public enum BoardCatalogUpdateMode
    {
        /// <summary>
        /// Загрузить.
        /// </summary>
        Load,

        /// <summary>
        /// Получить из кэша.
        /// </summary>
        GetFromCache,
    }
}