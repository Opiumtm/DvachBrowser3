using DvachBrowser3.Logic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Запрос на поиск по хэшу.
    /// </summary>
    public sealed class LinkHashPostCollectionSearchQuery : IPostCollectionSearchQuery
    {
        private readonly ILinkHashService linkHashService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="linkHash">Хэш ссылки.</param>
        public LinkHashPostCollectionSearchQuery(string linkHash)
        {
            LinkHash = linkHash;
            linkHashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
        }

        /// <summary>
        /// Хэш ссылки.
        /// </summary>
        public string LinkHash { get; }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Результат фильтрации.</returns>
        public bool Filter(IPostViewModel post)
        {
            if (post?.Link == null || LinkHash == null)
            {
                return false;
            }
            return linkHashService.GetLinkHash(post.Link) == LinkHash;
        }
    }
}