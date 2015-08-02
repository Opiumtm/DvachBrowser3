using System.Collections.Generic;
using System.Windows.Input;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Фильтр постов.
    /// </summary>
    public interface IPostFiltering : IPageStateAware
    {
        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        IPostCollectionViewModel Parent { get; }

        /// <summary>
        /// Фильтры.
        /// </summary>
        IList<IPostCollectionFilterMode> Filters { get; }

        /// <summary>
        /// Текущий фильтр.
        /// </summary>
        IPostCollectionFilterMode CurrentFilter { get; set; }

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
        /// Скрывать отфильтрованные посты.
        /// </summary>
        bool HideFilteredPosts { get; set; }
    }
}