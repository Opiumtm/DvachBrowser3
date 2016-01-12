using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using DvachBrowser3.Styles;

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
        /// Добавить борду.
        /// </summary>
        /// <param name="board">Борда.</param>
        void Remove(IBoardListBoardViewModel board);

        /// <summary>
        /// Есть группы.
        /// </summary>
        bool HasGroups { get; }

        /// <summary>
        /// Обновить список борд.
        /// </summary>
        IOperationViewModel Refresh { get; }

        /// <summary>
        /// Фильтр.
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Применить фильтр.
        /// </summary>
        void ApplyFilter();

        /// <summary>
        /// Добавить в избранное.
        /// </summary>
        ICommand AddToFavorites { get; }

        /// <summary>
        /// Убрать из избранного.
        /// </summary>
        ICommand RemoveFromFavorites { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        IStyleManager StyleManager { get; }
    }
}