using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Engines.Makaba.BoardInfo;
using DvachBrowser3.Engines.Makaba.Html;
using DvachBrowser3.SystemInformation;

namespace DvachBrowser3.Services
{
    /// <summary>
    /// Инициаизация сервисов Makaba.
    /// </summary>
    public static class MakabaEngineServicesInitializer
    {
        /// <summary>
        /// Инициализатор сервисов.
        /// </summary>
        /// <param name="sysInfo">Информация о системе.</param>
        /// <param name="container">Сервисы.</param>
        public static void InitializeServices(ServiceContainer container, SystemInfoParam sysInfo)
        {
            container.RegisterService<IMakabaUriService>(new MakabaUriService(container));
            container.RegisterService<IMakabaHtmlPostParseService>(new MakabaHtmlPostParseService(container));
            container.RegisterService<IMakabaJsonResponseParseService>(new MakabaJsonResponseParseService(container));
            container.RegisterService<IMakabaBoardInfoParser>(new MakabaBoardInfoParser(container));
            container.GetServiceOrThrow<INetworkEngineInstaller>().Install(new MakabaEngine(container));
        }
    }
}