using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.Engines.Makaba.Html;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Операция загрузки каталога.
    /// </summary>
    public sealed class MakabaGetCatalogOperation : HttpGetJsonEngineOperationBase<IThreadResult, MakabaCatalogArgument, CatalogEntity>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetCatalogOperation(MakabaCatalogArgument parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            var uri = Services.GetServiceOrThrow<IMakabaUriService>().GetCatalogUri(Parameter.Link, Parameter.Sort);
            if (uri == null)
            {
                throw new ArgumentException("Неправильный формат ссылки (get catalog)");
            }
            return uri;
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected async override Task<IThreadResult> ProcessJson(CatalogEntity message, string etag, CancellationToken token)
        {
            var task = Task<IThreadResult>.Factory.StartNew(() =>
            {
                var data = new OperationResult()
                {
                    CollectionResult = Services.GetServiceOrThrow<IMakabaJsonResponseParseService>().ParseCatalogTree(message, Parameter.Link)
                };
                return data;
            });
            return await task;
        }

        private class OperationResult : IThreadResult
        {
            public PostTreeCollection CollectionResult { get; set; }
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="filter">Фильтр.</param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
        }

    }
}