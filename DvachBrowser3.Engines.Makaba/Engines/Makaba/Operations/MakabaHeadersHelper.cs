using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Класс помощник для хидеров запроса.
    /// </summary>
    public static class MakabaHeadersHelper
    {
        /// <summary>
        /// Установить заголовки.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="client">HTTP клиент.</param>
        /// <param name="filer"></param>
        /// <returns>Таск.</returns>
        public static async Task SetClientHeaders(IServiceProvider services, HttpClient client, IHttpFilter filer)
        {
            var httpFilter = filer as HttpBaseProtocolFilter;
            if (httpFilter != null)
            {
                var makabaEngineConfig = services.GetServiceOrThrow<INetworkEngines>().GetEngineById(CoreConstants.Engine.Makaba).Configuration as IMakabaEngineConfig;
                if (makabaEngineConfig != null)
                {
                    var baseUri = makabaEngineConfig.BaseUri;
                    var cookies = httpFilter.CookieManager.GetCookies(baseUri);
                    foreach (var kv in cookies)
                    {
                        client.DefaultRequestHeaders.Cookie.Add(new HttpCookiePairHeaderValue(kv.Name, kv.Value));
                    }
                }
            }
            var engines = services.GetServiceOrThrow<INetworkEngines>();
            var makaba = engines.GetEngineById(CoreConstants.Engine.Makaba);
            var config = (IMakabaEngineConfig)makaba.Configuration;
            var agent = config.BrowserUserAgent;
            if (!string.IsNullOrWhiteSpace(agent))
            {
                client.DefaultRequestHeaders["User-Agent"] = agent;
            }
        }
    }
}