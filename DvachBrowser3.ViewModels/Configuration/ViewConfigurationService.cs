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
        }

        /// <summary>
        /// Конфигурация отображения.
        /// </summary>
        public IViewConfiguration View { get; private set; }
    }
}