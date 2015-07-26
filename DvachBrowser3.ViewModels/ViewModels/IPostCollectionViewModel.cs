using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public interface IPostCollectionViewModel : ICancellationTokenSource, IPageStateAware
    {
        /// <summary>
        /// Тип коллекции.
        /// </summary>
        PostCollectionKind Kind { get; }

        /// <summary>
        /// Источник постов.
        /// </summary>
        IPostCollectionSource CollectionSource { get; }

        /// <summary>
        /// Данные.
        /// </summary>
        PostTreeCollection Data { get; }

        /// <summary>
        /// Посты.
        /// </summary>
        IList<IPostViewModel> Posts { get; }

        /// <summary>
        /// Посты загружены.
        /// </summary>
        bool PostsLoaded { get; }

        /// <summary>
        /// Навигация по постам.
        /// </summary>
        IPostNavigation PostNavigation { get; }

        /// <summary>
        /// Найти пост.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Пост.</returns>
        IPostViewModel FindPost(BoardLinkBase link);

        /// <summary>
        /// Поинт постинга.
        /// </summary>
        IPostingPointHost PostingPoint { get; }
    }
}