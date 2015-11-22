using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.Board;
using DvachBrowser3.Engines.Makaba.BoardInfo;
using DvachBrowser3.Engines.Makaba.Json;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    using MobileBoardInfoGroups = Dictionary<string, MobileBoardInfo[]>;

    /// <summary>
    /// Операция получения списка борд.
    /// </summary>
    public sealed class MakabaGetBoardsOperation : HttpGetJsonEngineOperationBase<IBoardListResult, Empty, MobileBoardInfoGroups>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetBoardsOperation(Empty parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetBoardsListUri();
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected override async Task<IBoardListResult> ProcessJson(MobileBoardInfoGroups message, string etag, CancellationToken token)
        {
            var service = Services.GetServiceOrThrow<IMakabaBoardInfoParser>();
            var result = new List<BoardReference>();
            foreach (var kv in message)
            {
                foreach (var b in kv.Value)
                {
                    result.Add(service.Parse(kv.Key, b));
                }
            }
            return new OperationResult() {Boards = result};
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

        private class OperationResult : IBoardListResult
        {
            public List<BoardReference> Boards { get; set; }
        }
    }
}