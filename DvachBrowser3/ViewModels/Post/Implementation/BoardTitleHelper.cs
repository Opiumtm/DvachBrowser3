using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Класс-помощник получения имени борды.
    /// </summary>
    public static class BoardTitleHelper
    {
        /// <summary>
        /// Получить заголовок борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Заголовок.</returns>
        public static async Task<string> GetBoardTitle(BoardLinkBase link)
        {
            if (link == null)
            {
                return "";
            }
            var engine = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>().FindEngine(link.Engine);
            if (engine == null)
            {
                return "";
            }
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();

            var boardLink = linkTransform.BoardLinkFromAnyLink(link);
            if (boardLink == null)
            {
                return "";
            }

            var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();

            var hash = linkHash.GetLinkHash(boardLink);

            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();

            string title = null;

            var favData = (await storage.ThreadData.FavoriteBoards.LoadLinkCollectionForReadOnly()) as BoardLinkCollection;
            if (favData?.BoardInfo != null)
            {
                if (favData.BoardInfo.ContainsKey(hash))
                {
                    title = favData.BoardInfo[hash].DisplayName;
                }
            }

            if (title == null)
            {
                var rootLink = engine.RootLink;

                var boards = await storage.ThreadData.LoadBoardReferences(rootLink);

                if (boards?.References != null)
                {
                    var data = boards.References.FirstOrDefault(l => hash.Equals(linkHash.GetLinkHash(l?.Link)));
                    if (data != null)
                    {
                        title = data.DisplayName;
                    }
                }
            }

            if (title != null)
            {
                return $"/{linkTransform.GetBoardShortName(boardLink)}/ - {title.Trim()}";
            }
            return $"/{linkTransform.GetBoardShortName(boardLink)}/";
        }
    }
}