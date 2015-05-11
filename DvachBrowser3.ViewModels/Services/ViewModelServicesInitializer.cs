using DvachBrowser3.Configuration;
using DvachBrowser3.Navigation;
using DvachBrowser3.SystemInformation;

namespace DvachBrowser3
{
    /// <summary>
    /// Инициализатор сервисов моделей представления.
    /// </summary>
    public static class ViewModelServicesInitializer
    {
        /// <summary>
        /// Инициализатор сервисов.
        /// </summary>
        /// <param name="sysInfo">Информация о системе.</param>
        /// <param name="container">Сервисы.</param>
        public static void InitializeServices(ServiceContainer container, SystemInfoParam sysInfo)
        {
            container.RegisterService<INavigationKeyService>(new NavigationKeyService(container));
            container.RegisterService<INavigationService>(new NavigationService(container));
            container.RegisterService<IViewConfigurationService>(new ViewConfigurationService(container));
            container.RegisterService<IBoardLinkKeyService>(new BoardLinkKeyService(container));
        }
    }
}