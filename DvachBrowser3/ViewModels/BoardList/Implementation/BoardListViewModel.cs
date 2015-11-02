using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using Template10.Common;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления списка борд.
    /// </summary>
    public sealed class BoardListViewModel : ViewModelBase, IBoardListViewModel
    {
        private readonly IEqualityComparer<BoardLinkBase> linkEqualityComparer;

        private readonly IComparer<BoardLinkBase> linkComparer;

        private Dictionary<BoardLinkBase, IBoardListBoardViewModel> references;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BoardListViewModel()
        {
            Filter = "";
            refreshCommand.ResultGot += RefreshCommandOnResultGot;
            linkEqualityComparer = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetComparer();
            linkComparer = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
            references = new Dictionary<BoardLinkBase, IBoardListBoardViewModel>(linkEqualityComparer);
        }

        private async void RefreshCommandOnResultGot(object sender, EventArgs eventArgs)
        {
            try
            {
                await BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(() => UpdateList());
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task<ICollection<IBoardListBoardViewModel>> GetFavorites()
        {
            try
            {
                var result = await ServiceLocator.Current.GetServiceOrThrow<IStorageService>().ThreadData.FavoriteBoards.LoadLinkCollection();
                var resultList = new List<IBoardListBoardViewModel>();
                var linkHashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                Dictionary<string, FavoriteBoardInfo> favInfo;
                if (result is BoardLinkCollection)
                {
                    favInfo = ((BoardLinkCollection) result).BoardInfo;
                }
                else
                {
                    favInfo = new Dictionary<string, FavoriteBoardInfo>();
                }
                if (result?.Links != null)
                {
                    foreach (var l in result.Links)
                    {
                        string desc = null;
                        var hash = linkHashService.GetLinkHash(l);
                        if (favInfo.ContainsKey(hash))
                        {
                            desc = favInfo[hash].DisplayName;
                        }
                        if (desc == null)
                        {
                            if (references.ContainsKey(l))
                            {
                                desc = references[l].DisplayName;
                            }
                        }
                        if (desc == null)
                        {
                            desc = "";
                        }
                        resultList.Add(new BoardListBoardDataViewModel(l, desc, "Избранные", true));
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return new List<IBoardListBoardViewModel>();
            }
        }

        private async void UpdateList()
        {
            OriginalGroups.Clear();
            Groups.Clear();
            var result = (refreshCommand.Result ?? new List<IBoardListBoardViewModel>()).Where(i => i.Link != null).ToArray();
            references = result.Deduplicate(i => i.Link, linkEqualityComparer).ToDictionary(i => i.Link, linkEqualityComparer);
            var favs = await GetFavorites();
            var toResult = result.Concat(favs);
            var byCat = toResult.GroupBy(i => new BoardCategoryKey(i), BoardCategoryKey.EqualityComparer);
            foreach (var gc in byCat.OrderBy(gc => gc.Key, BoardCategoryKey.Comparer))
            {
                var g = new BoardListGroupingViewModel(gc.Key.Name, gc.Key.IsFavorite);
                foreach (var v in gc.OrderBy(v => v.Link, linkComparer))
                {
                    g.OriginalItems.Add(v);
                    g.Items.Add(v);
                }
                OriginalGroups.Add(g);
                Groups.Add(g);
            }

            ApplyFilter();
        }

        /// <summary>
        /// Группы.
        /// </summary>
        public IList<IBoardListBoardGroupingViewModel> Groups { get; } = new ObservableCollection<IBoardListBoardGroupingViewModel>();

        /// <summary>
        /// Группы до фильтрации.
        /// </summary>
        public IList<IBoardListBoardGroupingViewModel> OriginalGroups { get; } = new ObservableCollection<IBoardListBoardGroupingViewModel>();

        /// <summary>
        /// Добавить борду.
        /// </summary>
        /// <param name="board">Борда.</param>
        public void Add(IBoardListViewModel board)
        {
        }

        /// <summary>
        /// Есть группы.
        /// </summary>
        public bool HasGroups => Groups.Count > 0;

        /// <summary>
        /// Фильтр.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Применить фильтр.
        /// </summary>
        public void ApplyFilter()
        {
            foreach (var g in OriginalGroups)
            {
                g.ApplyFilter(Filter);
            }
            var newGroups = OriginalGroups.Where(g => g.HasItems).OrderBy(g => new BoardCategoryKey(g), BoardCategoryKey.Comparer).ToArray();
            var updateHelper = new SortedCollectionUpdateHelper<IBoardListBoardGroupingViewModel, IBoardListBoardGroupingViewModel, BoardCategoryKey>(
                BoardCategoryKey.EqualityComparer,
                BoardCategoryKey.Comparer,
                a => new BoardCategoryKey(a), 
                a => new BoardCategoryKey(a),
                a => a,
                (a, b) => true,
                (a, b) => { },
                newGroups,
                Groups
                );
            var update = updateHelper.GetUpdate();
            update.Update();
            RaisePropertyChanged(nameof(HasGroups));
        }

        private readonly StdEngineOperationWrapper<ICollection<IBoardListBoardViewModel>> refreshCommand = new StdEngineOperationWrapper<ICollection<IBoardListBoardViewModel>>(RefreshOperationFactory);

        /// <summary>
        /// Обновить список борд.
        /// </summary>
        public IOperationViewModel Refresh => refreshCommand;

        private static IEngineOperationsWithProgress<ICollection<IBoardListBoardViewModel>, EngineProgress> RefreshOperationFactory(object o)
        {
            var force = true;
            if (o is bool)
            {
                force = (bool) o;
            }
            return new SyncBoardsOperation(force);
        }

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Start()
        {
            await BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(() =>
            {
                refreshCommand.Start(false);
            });
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Stop()
        {
            await BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(() =>
            {
                refreshCommand.Cancel();
            });
        }
    }
}