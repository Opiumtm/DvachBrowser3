namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Сервис конфигурации UI.
    /// </summary>
    public interface IUiConfigurationService
    {
        /// <summary>
        /// Страницы UI.
        /// </summary>
        IUiPagesConfiguration UiPages { get; }         
    }
}