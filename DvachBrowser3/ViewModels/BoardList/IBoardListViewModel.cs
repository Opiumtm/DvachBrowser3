using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Windows.UI.Xaml.Data;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления списка борд.
    /// </summary>
    public interface IBoardListViewModel : IStartableViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Группы.
        /// </summary>
        IList<IBoardListBoardGroupingViewModel> Groups { get; }

        /// <summary>
        /// Добавить борду.
        /// </summary>
        /// <param name="board">Борда.</param>
        void Add(IBoardListBoardViewModel board);

        /// <summary>
        /// Есть группы.
        /// </summary>
        bool HasGroups { get; }

        /// <summary>
        /// Обновить список борд.
        /// </summary>
        IOperationViewModel Refresh { get; }
    }
}