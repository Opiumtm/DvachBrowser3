using DvachBrowser3.Links;
using DvachBrowser3.Logic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Запрос на поиск по ссылке.
    /// </summary>
    public sealed class LinkPostCollectionSearchQuery : IPostCollectionSearchQuery
    {
        private readonly ILinkHashService linkHashService;

        private readonly string linkHash;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public LinkPostCollectionSearchQuery(BoardLinkBase link)
        {
            Link = link;
            linkHashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
            linkHash = Link != null ? linkHashService.GetLinkHash(Link) : null;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Результат фильтрации.</returns>
        public bool Filter(IPostViewModel post)
        {
            if (post?.Link == null || linkHash == null)
            {
                return false;
            }
            return linkHashService.GetLinkHash(post.Link) == linkHash;
        }
    }
}