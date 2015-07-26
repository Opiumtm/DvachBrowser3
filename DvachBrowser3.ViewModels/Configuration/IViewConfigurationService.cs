namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Сервис конфигурации отображения.
    /// </summary>
    public interface IViewConfigurationService
    {
        /// <summary>
        /// Конфигурация отображения.
        /// </summary>
        IViewConfiguration View { get; }

        /// <summary>
        /// Конфигурация профиля сети.
        /// </summary>
        INetworkProfileConfiguration NetworkProfile { get; }
    }
}