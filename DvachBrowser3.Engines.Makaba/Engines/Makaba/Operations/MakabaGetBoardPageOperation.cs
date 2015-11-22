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
    /// Получить страницу.
    /// </summary>
    public sealed class MakabaGetBoardPageOperation : HttpGetJsonEngineOperationBase<IBoardPageResult, BoardLinkBase, BoardEntity2>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetBoardPageOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        private BoardPageLink GetPageLink()
        {
            BoardPageLink pageLink;
            if (Parameter is BoardLink)
            {
                pageLink = new BoardPageLink() { Engine = CoreConstants.Engine.Makaba, Board = ((BoardLink)Parameter).Board, Page = 0 };
            }
            else if (Parameter is BoardPageLink)
            {
                var l = Parameter as BoardPageLink;
                pageLink = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Page = l.Page
                };
            }
            else if (Parameter is ThreadLink)
            {
                var l = Parameter as ThreadLink;
                pageLink = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Page = 0
                };                
            }
            else if (Parameter is PostLink)
            {
                var l = Parameter as PostLink;
                pageLink = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Page = 0
                };
            }
            else if (Parameter is BoardMediaLink)
            {
                var l = Parameter as BoardMediaLink;
                pageLink = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Page = 0
                };                
            }
            else
            {
                throw new ArgumentException("Неправильный формат ссылки (get board page)");
            }
            return pageLink;
        }

        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetJsonLink(GetPageLink());
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected override async Task<IBoardPageResult> ProcessJson(BoardEntity2 message, string etag, CancellationToken token)
        {
            var task = Task<IBoardPageResult>.Factory.StartNew(() =>
            {
                var data = Services.GetServiceOrThrow<IMakabaJsonResponseParseService>().ParseBoardPage(message, GetPageLink());
                return new OperationResult()
                {
                    Result = data
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

        private class OperationResult : IBoardPageResult
        {
            public BoardPageTree Result { get; set; }
        }
    }
}