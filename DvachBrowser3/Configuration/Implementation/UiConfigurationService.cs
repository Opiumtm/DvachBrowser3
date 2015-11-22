using System;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Сервис конфигурации UI.
    /// </summary>
    public class UiConfigurationService : ServiceBase, IUiConfigurationService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public UiConfigurationService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Страницы UI.
        /// </summary>
        public IUiPagesConfiguration UiPages { get; } = new UiPagesConfiguration();
    }
}