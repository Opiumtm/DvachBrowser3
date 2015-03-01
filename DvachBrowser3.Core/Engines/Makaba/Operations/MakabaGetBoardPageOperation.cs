using System;
using System.Threading.Tasks;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;

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

        protected override Uri GetRequestUri()
        {
            BoardPageLink pageLink;
            if (Parameter is BoardLink)
            {
                pageLink = new BoardPageLink() {Board = ((BoardLink) Parameter).Board, Page = 0};
            } else if (Parameter is BoardPageLink)
            {
                pageLink = Parameter as BoardPageLink;
            }
            else
            {
                throw new ArgumentException("Неправильный формат ссылки (get board page)");
            }
            return Services.GetServiceOrThrow<IMakabaUriService>().GetBoardPageUri(pageLink, false);
        }

        protected override Task<IBoardPageResult> ProcessJson(BoardEntity2 message, string etag)
        {
            throw new NotImplementedException();
        }
    }
}