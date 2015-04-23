using System;
using System.Threading.Tasks;
using Windows.Web.Http;
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
        /// <returns>Таск.</returns>
        public static async Task SetClientHeaders(IServiceProvider services, HttpClient client)
        {
            var engines = services.GetServiceOrThrow<INetworkEngines>();
            var makaba = engines.GetEngineById(CoreConstants.Engine.Makaba);
            var config = (IMakabaEngineConfig)makaba.Configuration;
            var cookies = await config.GetCookies();
            foreach (var kv in cookies)
            {
                client.DefaultRequestHeaders[kv.Key] = kv.Value;
            }
            client.DefaultRequestHeaders.AcceptEncoding.Add(new HttpContentCodingWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders["User-Agent"] = config.BrowserUserAgent;
        }
    }
}