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
    }
}