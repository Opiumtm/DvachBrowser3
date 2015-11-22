using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.Engines.Makaba.Html;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить тред целиком.
    /// </summary>
    public sealed class MakabaGetThreadOperation : HttpGetJsonEngineOperationBase<IThreadResult, BoardLinkBase, BoardEntity2>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметры.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetThreadOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        private ThreadLink GetThreadLink()
        {
            if (Parameter is ThreadPartLink)
            {
                var l = Parameter as ThreadLink;
                return new ThreadLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Thread = l.Thread
                };
            }
            if (Parameter is ThreadLink)
            {
                var l = Parameter as ThreadLink;
                return new ThreadLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Thread = l.Thread
                };
            }
            if (Parameter is PostLink)
            {
                var l = Parameter as PostLink;
                return new ThreadLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Thread = l.Thread
                };                
            }
            throw new ArgumentException("Неправильный формат ссылки (get thread)");
        }

        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetJsonLink(GetThreadLink());
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected override async Task<IThreadResult> ProcessJson(BoardEntity2 message, string etag, CancellationToken token)
        {
            var task = Task<IThreadResult>.Factory.StartNew(() =>
            {
                var data = Services.GetServiceOrThrow<IMakabaJsonResponseParseService>().ParseThread(message, GetThreadLink());
                return new OperationResult()
                {
                    CollectionResult = data
                };
            });
            return await task;
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

        private class OperationResult : IThreadResult
        {
            public PostTreeCollection CollectionResult { get; set; }
        }
    }
}