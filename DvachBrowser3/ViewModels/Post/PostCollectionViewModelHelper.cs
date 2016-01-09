using System.Collections.Generic;
using System.Linq;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Класс-помощник для модели представления списка постов.
    /// </summary>
    public static class PostCollectionViewModelHelper
    {
        /// <summary>
        /// Найти пост.
        /// </summary>
        /// <param name="src">Спиок постов.</param>
        /// <param name="query">Запрос.</param>
        /// <returns>Результат.</returns>
        public static IPostViewModel FindPost(this IPostCollectionViewModel src, IPostCollectionSearchQuery query)
        {
            return src.Posts.FirstOrDefault(query.Filter);
        }

        /// <summary>
        /// Получить отфильтрованный список постов.
        /// </summary>
        /// <param name="src">Список постов.</param>
        /// <param name="query">Запрос.</param>
        /// <param name="sortPosts">Сортировать посты.</param>
        /// <returns>Результат.</returns>
        public static IFilteredPostCollection FilterPosts(this IPostCollectionViewModel src, IPostCollectionSearchQuery query, bool sortPosts = true)
        {
            return new FilteredPostCollection(src, query, sortPosts);
        }
    }
}