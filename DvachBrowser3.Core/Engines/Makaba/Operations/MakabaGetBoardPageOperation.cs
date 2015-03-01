using System;
using System.Threading.Tasks;
using DvachBrowser3.Engines.Makaba.Html;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить страницу.
    /// </summary>
    public class MakabaGetBoardPageOperation : HttpGetJsonEngineOperationBase<IBoardPageResult, BoardLinkBase, BoardEntity2>
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
                pageLink = new BoardPageLink() { Board = ((BoardLink)Parameter).Board, Page = 0 };
            }
            else if (Parameter is BoardPageLink)
            {
                pageLink = Parameter as BoardPageLink;
            }
            else
            {
                throw new ArgumentException("Неправильный формат ссылки (get board page)");
            }
            return pageLink;
        }

        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetBoardPageUri(GetPageLink(), false);
        }

        protected override async Task<IBoardPageResult> ProcessJson(BoardEntity2 message, string etag)
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

        private class OperationResult : IBoardPageResult
        {
            public BoardPageTree Result { get; set; }
        }
    }
}