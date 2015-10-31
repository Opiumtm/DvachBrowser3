using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Операция получения последнего изменения.
    /// </summary>
    public sealed class MakabaLastModifiedOperation : HttpEngineHeadJsonEngineOperationBase<ILastModifiedCheckResult, BoardLinkBase>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaLastModifiedOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            var uri = Services.GetService<IMakabaUriService>().GetHtmlLink(Parameter);
            if (uri != null)
            {
                return uri;
            }
            throw new ArgumentException("Неправильный формат ссылки (last-modified)");
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
                return new OperationResult() {LastModified = message.Headers["ETag"]};
            }
            return new OperationResult() {LastModified = null};
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client)
        {
            await base.SetHeaders(client);
            await MakabaHeadersHelper.SetClientHeaders(Services, client);
        }

        private class OperationResult : ILastModifiedCheckResult
        {
            public string LastModified { get; set; }
        }
    }
}