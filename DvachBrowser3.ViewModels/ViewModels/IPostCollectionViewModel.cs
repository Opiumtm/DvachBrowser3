using System;
using System.Collections.Generic;
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
        /// Фильтры.
        /// </summary>
        IList<IPostCollectionFilterMode> Filters { get; }

        /// <summary>
        /// Текущий фильтр.
        /// </summary>
        IPostCollectionFilterMode CurrentFilter { get; }

        /// <summary>
        /// Сбросить фильтр (установить фильтр по умолчанию).
        /// </summary>
        void ResetFilter();

        /// <summary>
        /// Команда сброса фильтра.
        /// </summary>
        ICommand ResetFilterCommand { get; }

        /// <summary>
        /// Обновить фильтр.
        /// </summary>
        void RefreshFilter();

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
    }
}