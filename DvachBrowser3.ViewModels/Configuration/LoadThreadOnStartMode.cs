namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Режим загрузки треда.
    /// </summary>
    public enum LoadThreadOnStartMode : int
    {
        /// <summary>
        /// Не загружать.
        /// </summary>
        None = 0,

        /// <summary>
        /// Проверить на обновление.
        /// </summary>
        CheckForUpdates = 1,

        /// <summary>
        /// Загружать.
        /// </summary>
        Load = 2
    }
}