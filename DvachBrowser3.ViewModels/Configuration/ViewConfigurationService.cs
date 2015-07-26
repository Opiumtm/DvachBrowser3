using System;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Сервис конфигурации отображения.
    /// </summary>
    public sealed class ViewConfigurationService : ServiceBase, IViewConfigurationService
    {        
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public ViewConfigurationService(IServiceProvider services) : base(services)
        {
            View = new ViewConfiguration();
            NetworkProfile = new NetworkProfileConfiguration();
        }

        /// <summary>
        /// Конфигурация отображения.
        /// </summary>
        public IViewConfiguration View { get; private set; }

        /// <summary>
        /// Конфигурация профиля сети.
        /// </summary>
        public INetworkProfileConfiguration NetworkProfile { get; private set; }
    }
}