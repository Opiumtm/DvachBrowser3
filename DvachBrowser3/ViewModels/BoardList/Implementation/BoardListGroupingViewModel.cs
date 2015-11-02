using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группировка.
    /// </summary>
    public sealed class BoardListGroupingViewModel : ViewModelBase, IBoardListBoardGroupingViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя группы.</param>
        /// <param name="isFavorite">Избранное.</param>
        public BoardListGroupingViewModel(string name, bool isFavorite)
        {
            IsFavorite = isFavorite;
            Name = name;
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Элементы.
        /// </summary>
        public IList<IBoardListBoardViewModel> Items { get; } = new ObservableCollection<IBoardListBoardViewModel>();

        /// <summary>
        /// До фильтрации.
        /// </summary>
        public IList<IBoardListBoardViewModel> OriginalItems { get; } = new ObservableCollection<IBoardListBoardViewModel>();

        /// <summary>
        /// Избранные.
        /// </summary>
        public bool IsFavorite { get; private set; }

        /// <summary>
        /// Есть элементы.
        /// </summary>
        public bool HasItems => Items.Count > 0;

        /// <summary>
        /// Применить фильтр.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        public void ApplyFilter(string filter)
        {
            var comparer = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
            var newItems = Items.Where(i => i.Filter(filter)).OrderBy(i => i.Link, comparer).ToArray();
            var updateHelper =
                new SortedCollectionUpdateHelper<IBoardListBoardViewModel, IBoardListBoardViewModel, BoardLinkBase>(
                        ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetComparer(),
                        comparer,
                        a => a.Link,
                        a => a.Link,
                        a => a,
                        (a,b) => true,
                        null,
                        newItems,
                        Items                        
                    );
            var update = updateHelper.GetUpdate();
            update.Update();
            RaisePropertyChanged(nameof(HasItems));
        }
    }
}