using System.Collections.Generic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группировка списка борд.
    /// </summary>
    public interface IBoardListBoardGroupingViewModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Элементы.
        /// </summary>
        IList<IBoardListBoardViewModel> Items { get; }

        /// <summary>
        /// До фильтрации.
        /// </summary>
        IList<IBoardListBoardViewModel> OriginalItems { get; }

        /// <summary>
        /// Есть элементы.
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Применить фильтр.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        void ApplyFilter(string filter);
    }
}