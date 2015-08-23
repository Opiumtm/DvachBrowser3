using DvachBrowser.Core.Lifecycle;
using DvachBrowser3;
using DvachBrowser3.Lifecycle;
using DvachBrowser3.SystemInformation;

namespace DvachBrowser.Core.Services
{
    /// <summary>
    /// Инициализатор основных сервисов Windows 10.
    /// </summary>
    public class CoreWindows10ServicesInitializer
    {
        /// <summary>
        /// Инициализатор сервисов.
        /// </summary>
        /// <param name="sysInfo">Информация о системе.</param>
        /// <param name="container">Сервисы.</param>
        public static void InitializeServices(ServiceContainer container, SystemInfoParam sysInfo)
        {
            container.RegisterService<INavigationHelperFactory>(new NavigationHelperFactory(container));
        }
    }
}