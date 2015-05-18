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
        /// Перейти к посту.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        void GotoPost(BoardLinkBase link);

        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="number">Номер.</param>
        void GotoPost(int number);

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
        IPostFlagsViewModel CurrentFilter { get; }

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
        /// Пост попал в отображение.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        void PostGotIntoView(BoardLinkBase id);

        /// <summary>
        /// Отобразить пост.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        void BringIntoView(BoardLinkBase id);

        /// <summary>
        /// Нужно отобразить пост.
        /// </summary>
        event BringIntoViewEventHandler NeedBringIntoView;

        /// <summary>
        /// Верхний отображаемый пост.
        /// </summary>
        BoardLinkBase TopViewPost { get; }
    }
}