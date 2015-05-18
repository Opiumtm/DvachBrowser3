namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поддерживаемые апдейты коллекции.
    /// </summary>
    public enum SupportedPostCollecetionUpdates
    {
        /// <summary>
        /// Обновления не поддерживаются.
        /// </summary>
        None,

        /// <summary>
        /// Только полные обновления.
        /// </summary>
        FullOnly,

        /// <summary>
        /// Полные и частичные.
        /// </summary>
        FullAndPartial
    }
}