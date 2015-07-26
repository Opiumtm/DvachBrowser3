using System;

namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// Поиск по автору.
    /// </summary>
    public sealed class PosterFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        public PosterFilterMode(IPostFiltering parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get { return "poster"; }
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public override string Name
        {
            get { return "По автору"; }
        }

        /// <summary>
        /// Есть строка фильтра.
        /// </summary>
        public override bool HasFilterString
        {
            get { return true; }
        }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>true, если нужно отобразить пост.</returns>
        public override bool FilterPost(IPostViewModel post)
        {            
            if (string.IsNullOrWhiteSpace(Filter))
            {
                return true;
            }
            if (post.Poster == null)
            {
                return false;
            }
            var f = Filter.Trim();
            return (post.Poster.Name ?? "").IndexOf(f, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}