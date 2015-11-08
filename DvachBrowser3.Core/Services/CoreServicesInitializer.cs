using DvachBrowser3.ApiKeys;
using DvachBrowser3.Captcha;
using DvachBrowser3.Common;
using DvachBrowser3.Engines;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Storage;
using DvachBrowser3.SystemInformation;

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
        /// <param name="sysInfo">Информация о системе.</param>
        /// <param name="container">Сервисы.</param>
        public static void InitializeServices(ServiceContainer container, SystemInfoParam sysInfo)
        {
            container.RegisterService<IRegexCacheService>(new RegexCacheService(container));
            container.RegisterService<IYoutubeIdService>(new YoutubeIdService(container));
            container.RegisterService<IDateService>(new DateService(container));
            container.RegisterService<ICaptchaService>(new CaptchaService(container));
            container.RegisterService<ILinkHashService>(new LinkHashService(container));
            container.RegisterService<ISerializerCacheService>(new SerializerCacheService(container));
            container.RegisterService<IStorageService>(new StorageService(container));
            container.RegisterService<ILinkTransformService>(new LinkTransformService(container));
            container.RegisterService<INetworkLogic>(new NetworkLogicService(container));
            container.RegisterService<ISystemInfo>(new SystemInfo(container, sysInfo));
            container.RegisterService<IThreadTreeProcessService>(new ThreadTreeProcessService(container));
            container.RegisterService<ILiveTileService>(new LiveTileService(container));
            container.RegisterService<IJsonService>(new JsonService(container));
            container.RegisterService<IYoutubeUriService>(new YoutubeUriService(container));
            container.RegisterService<IApiKeyService>(new ApiKeyService(container));
            container.RegisterService<INavigationKeyService>(new NavigationKeyService(container));
            container.RegisterService<IBoardLinkKeyService>(new BoardLinkKeyService(container));

            var engines = new NetworkEngines(container);
            container.RegisterService<INetworkEngines>(engines);
            container.RegisterService<INetworkEngineInstaller>(engines);
        }
    }
}