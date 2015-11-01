using System.Collections.Generic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления списка борд.
    /// </summary>
    public interface IBoardListViewModel
    {
        /// <summary>
        /// Группы.
        /// </summary>
        IList<IBoardListBoardGroupingViewModel> Groups { get; }

        /// <summary>
        /// Группы до фильтрации.
        /// </summary>
        IList<IBoardListBoardGroupingViewModel> OriginalGroups { get; }

        /// <summary>
        /// Добавить борду.
        /// </summary>
        /// <param name="board">Борда.</param>
        void Add(IBoardListViewModel board);

        /// <summary>
        /// Есть группы.
        /// </summary>
        bool HasGroups { get; }

        /// <summary>
        /// Фильтр.
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Применить фильтр.
        /// </summary>
        void ApplyFilter();

        /// <summary>
        /// Обновить список борд.
        /// </summary>
        IOperationViewModel Refresh { get; }
    }
}