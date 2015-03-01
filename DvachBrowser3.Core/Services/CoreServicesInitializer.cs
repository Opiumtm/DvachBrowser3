using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;

namespace DvachBrowser3
{
    /// <summary>
    /// Инициализатор основных сервисов.
    /// </summary>
    public static class CoreServicesInitializer
    {
        /// <summary>
        /// Инициализатор сервисов.
        /// </summary>
        /// <param name="container">Сервисы.</param>
        public static void InitializeServices(ServiceContainer container)
        {
            container.RegisterService<IRegexCacheService>(new RegexCacheService(container));
            container.RegisterService<IYoutubeIdService>(new YoutubeIdService(container));
            container.RegisterService<IMakabaUriService>(new MakabaUriService(container));
        }
    }
}