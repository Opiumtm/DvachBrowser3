using DvachBrowser3.Storage.Esent;
using DvachBrowser3.Storage.Files;
using DvachBrowser3.SystemInformation;

namespace DvachBrowser3
{
    /// <summary>
    /// Инициализатор сервисов.
    /// </summary>
    public static class EsentServicesInitializer
    {
        /// <summary>
        /// Инициализатор сервисов.
        /// </summary>
        /// <param name="sysInfo">Информация о системе.</param>
        /// <param name="container">Сервисы.</param>
        public static void InitializeServices(ServiceContainer container, SystemInfoParam sysInfo)
        {
            RegisterStorageSizeCache(container);
        }

        /// <summary>
        /// Зарегистрировать кэш размеров.
        /// </summary>
        /// <param name="container">Сервисы.</param>
        public static void RegisterStorageSizeCache(ServiceContainer container)
        {
            container.RegisterService<IStorageSizeCacheFactory>(new EsentStorageSizeCacheFactory(container));
        }
    }
}