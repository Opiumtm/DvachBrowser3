using DvachBrowser3.Logic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поиск по номеру поста.
    /// </summary>
    public sealed class PostNumPostCollectionSearchQuery : IPostCollectionSearchQuery
    {
        private readonly ILinkTransformService linkTransform;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="number">Номер поста.</param>
        public PostNumPostCollectionSearchQuery(int number)
        {
            Number = number;
            linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
        }

        /// <summary>
        /// Номер поста.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Результат фильтрации.</returns>
        public bool Filter(IPostViewModel post)
        {
            if (post?.Link == null)
            {
                return false;
            }
            var num = linkTransform.GetPostNum(post.Link);
            if (num == null)
            {
                return false;
            }
            return (post.Counter != null && post.Counter == Number) || num == Number;
        }
    }
}