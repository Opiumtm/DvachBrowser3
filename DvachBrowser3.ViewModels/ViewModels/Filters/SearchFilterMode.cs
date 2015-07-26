using System;
using System.Linq;
using DvachBrowser3.Engines.Makaba.Html;

namespace DvachBrowser3.ViewModels.Filters
{
    /// <summary>
    /// Поиск по строке.
    /// </summary>
    public sealed class SearchFilterMode : PostCollectionFilterModeBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        public SearchFilterMode(IPostFiltering parent) : base(parent)
        {
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get { return "search"; }
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public override string Name
        {
            get { return "Поиск"; }
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
            if (post == null)
            {
                return false;
            }
            var f = Filter.Trim();
            var text = MakabaJsonResponseParseService.ToPlainText(post.Data);
            return text.Any(l => (l ?? "").IndexOf(f, StringComparison.CurrentCultureIgnoreCase) >= 0) || ((post.Subject ?? "").IndexOf(f, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
    }
}