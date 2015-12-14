using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Операция получения последнего изменения каталога.
    /// </summary>
    public sealed class MakabaCatalogLastModifiedOperation : HttpEngineHeadJsonEngineOperationBase<ILastModifiedCheckResult, MakabaCatalogArgument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaCatalogLastModifiedOperation(MakabaCatalogArgument parameter, IServiceProvider services) : base(parameter, services)
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
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected async override Task<ILastModifiedCheckResult> DoComplete(HttpResponseMessage message, CancellationToken token)
        {
            if (message.Headers.ContainsKey("ETag"))
            {
                return new OperationResult() { LastModified = message.Headers["ETag"] };
            }
            return new OperationResult() { LastModified = null };
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

        private class OperationResult : ILastModifiedCheckResult
        {
            public string LastModified { get; set; }
        }
    }
}