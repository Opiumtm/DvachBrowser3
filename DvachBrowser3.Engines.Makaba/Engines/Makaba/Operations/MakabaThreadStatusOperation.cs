using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Операция получения статуса треда.
    /// </summary>
    public sealed class MakabaThreadStatusOperation : HttpGetJsonEngineOperationBase<IThreadStatusResult, BoardLinkBase, CheckUpdatesData>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaThreadStatusOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        protected override Uri GetRequestUri()
        {
            if (Parameter is ThreadLink)
            {
                return Services.GetService<IMakabaUriService>().GetLastThreadInfoUri((ThreadLink) Parameter);
            }
            if (Parameter is PostLink)
            {
                var p = (PostLink) Parameter;
                return Services.GetService<IMakabaUriService>().GetLastThreadInfoUri(new ThreadLink()
                {
                    Board = p.Board,
                    Thread = p.Thread
                });
            }
            throw new ArgumentException("Неправильный формат ссылки (get thread status)");
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected async override Task<IThreadStatusResult> ProcessJson(CheckUpdatesData message, string etag, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(message.ErrorMessage))
            {
                return new OperationResult()
                {
                    IsFound = true,
                    TotalPosts = message.Posts >= 0 ? (int?) message.Posts + 1 : null,
                    LastUpdate = Services.GetServiceOrThrow<IDateService>().FromUnixTime(message.TimeStamp),
                };
            }
            throw new WebException(string.Format("{0}: {1}", message.ErrorCode, message.ErrorMessage));
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="filter"></param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
        }

        private class OperationResult : IThreadStatusResult
        {
            public int? TotalPosts { get; set; }

            public DateTime LastUpdate { get; set; }

            public bool IsFound { get; set; }
        }
    }
}