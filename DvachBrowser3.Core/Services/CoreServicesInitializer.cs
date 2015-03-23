using DvachBrowser3.Captcha;
using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Engines.Makaba.BoardInfo;
using DvachBrowser3.Engines.Makaba.Html;

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
            container.RegisterService<IMakabaHtmlPostParseService>(new MakabaHtmlPostParseService(container));
            container.RegisterService<IDateService>(new DateService(container));
            container.RegisterService<IMakabaJsonResponseParseService>(new MakabaJsonResponseParseService(container));
            container.RegisterService<ICaptchaService>(new CaptchaService(container));
            container.RegisterService<IMakabaBoardInfoParser>(new MakabaBoardInfoParser(container));
            container.RegisterService<INetworkEngines>(new NetworkEngines(container));
        }
    }
}