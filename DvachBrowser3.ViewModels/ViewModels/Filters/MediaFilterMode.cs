using System.Linq;

namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// Фильтр по наличию медиа файлов.
    /// </summary>
    public sealed class MediaFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        public MediaFilterMode(IPostFiltering parent) : base(parent)
        {
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get { return "media"; }
        }

        public override string Name
        {
            get { return "С медиафайлами"; }
        }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>true, если нужно отобразить пост.</returns>
        public override bool FilterPost(IPostViewModel post)
        {
            if (post == null)
            {
                return false;
            }
            return post.Media.Any();
        }
    }
}