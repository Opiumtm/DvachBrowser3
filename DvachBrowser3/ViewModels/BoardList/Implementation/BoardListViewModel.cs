using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
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

        private ICollection<IBoardListBoardViewModel> cachedResult;

        private BoardLinkCollection cachedFavorites;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BoardListViewModel()
        {
            refreshCommand.ResultGot += RefreshCommandOnResultGot;
            linkEqualityComparer = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetComparer();
            linkComparer = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
            references = new Dictionary<BoardLinkBase, IBoardListBoardViewModel>(linkEqualityComparer);
            AddToFavorites = new DelegateCommand<IBoardListBoardViewModel>(Add);
            RemoveFromFavorites = new DelegateCommand<IBoardListBoardViewModel>(Remove);
        }

        private void RefreshCommandOnResultGot(object sender, ICollection<IBoardListBoardViewModel> result)
        {
            try
            {
                BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(() => UpdateList(result, cachedFavorites));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task<ICollection<IBoardListBoardViewModel>> GetFavorites(BoardLinkCollection linkCollection = null)
        {
            try
            {
                var result = linkCollection ?? (await ServiceLocator.Current.GetServiceOrThrow<IStorageService>().ThreadData.FavoriteBoards.LoadLinkCollectionForReadOnly()) as BoardLinkCollection;
                cachedFavorites = result;
                var resultList = new List<IBoardListBoardViewModel>();
                var linkHashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                if (result?.Links != null)
                {
                    var favInfo = result.BoardInfo;
                    foreach (var l in result.Links)
                    {
                        string desc = null;
                        var hash = linkHashService.GetLinkHash(l);
                        if (favInfo?.ContainsKey(hash) == true)
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
                        resultList.Add(new BoardListBoardDataViewModel(l, desc, "Избранные", true, false));
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

        private async void UpdateList(ICollection<IBoardListBoardViewModel> rresult, BoardLinkCollection favorites)
        {
            var newGroups = new List<IBoardListBoardGroupingViewModel>();
            var result = (rresult ?? new List<IBoardListBoardViewModel>()).Where(i => i.Link != null).ToArray();
            cachedResult = result;
            references = result.Deduplicate(i => i.Link, linkEqualityComparer).ToDictionary(i => i.Link, linkEqualityComparer);
            var favs = await GetFavorites(favorites);
            var toResult = result.Where(r => !r.IsAdult).Concat(favs).Where(DoFilter);
            var byCat = toResult.GroupBy(i => new BoardCategoryKey(i), BoardCategoryKey.EqualityComparer);
            foreach (var gc in byCat.OrderBy(gc => gc.Key, BoardCategoryKey.Comparer).ToArray())
            {
                var g = new BoardListGroupingViewModel(gc.Key.Name, gc.Key.IsFavorite);
                foreach (var v in gc.OrderBy(v => v.Link, linkComparer).ToArray())
                {
                    g.Add(v);
                }
                newGroups.Add(g);
            }

            Groups = newGroups;

            ViewModelEvents.BoardListRefresh.RaiseEvent(this, null);
        }

        private bool DoFilter(IBoardListBoardViewModel obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj.Filter(Filter);
        }

        private IList<IBoardListBoardGroupingViewModel> groups;

        /// <summary>
        /// Группы.
        /// </summary>
        public IList<IBoardListBoardGroupingViewModel> Groups
        {
            get { return groups; }
            private set
            {
                groups = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить борду.
        /// </summary>
        /// <param name="board">Борда.</param>
        public async void Add(IBoardListBoardViewModel board)
        {
            if (board?.Link == null)
            {
                return;
            }
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var oldData = await store.ThreadData.FavoriteBoards.LoadLinkCollection() as BoardLinkCollection ??
                              new BoardLinkCollection()
                              {
                                  Links = new List<BoardLinkBase>(),
                                  BoardInfo = new Dictionary<string, FavoriteBoardInfo>()
                              };
                var hashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var linkSet = new HashSet<string>(oldData.Links.Where(l => l != null).Select(l => hashService.GetLinkHash(l)));
                var linkHash = hashService.GetLinkHash(board.Link);
                if (!linkSet.Contains(linkHash))
                {
                    oldData.Links.Add(board.Link);
                }
                oldData.BoardInfo[linkHash] = new FavoriteBoardInfo()
                {
                    DisplayName = board.DisplayName
                };
                oldData.Links.Sort(linkComparer);
                await store.ThreadData.FavoriteBoards.SaveLinkCollectionSync(oldData);
                if (cachedResult != null)
                {
                    UpdateList(cachedResult, oldData);
                }
                else
                {
                    refreshCommand.Start2(false);
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Добавить борду.
        /// </summary>
        /// <param name="board">Борда.</param>
        public async void Remove(IBoardListBoardViewModel board)
        {
            if (board?.Link == null)
            {
                return;
            }
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var oldData = await store.ThreadData.FavoriteBoards.LoadLinkCollection() as BoardLinkCollection ??
                              new BoardLinkCollection()
                              {
                                  Links = new List<BoardLinkBase>(),
                                  BoardInfo = new Dictionary<string, FavoriteBoardInfo>()
                              };
                var hashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var linkHash = hashService.GetLinkHash(board.Link);
                oldData.Links = oldData.Links.Where(l => !linkEqualityComparer.Equals(l, board.Link)).ToList();
                if (oldData.BoardInfo.ContainsKey(linkHash))
                {
                    oldData.BoardInfo.Remove(linkHash);
                }
                oldData.Links.Sort(linkComparer);
                await store.ThreadData.FavoriteBoards.SaveLinkCollectionSync(oldData);
                if (cachedResult != null)
                {
                    UpdateList(cachedResult, oldData);
                }
                else
                {
                    refreshCommand.Start2(false);
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Есть группы.
        /// </summary>
        public bool HasGroups => Groups.Count > 0;

        private readonly StdEngineOperationWrapper<ICollection<IBoardListBoardViewModel>> refreshCommand = new StdEngineOperationWrapper<ICollection<IBoardListBoardViewModel>>(RefreshOperationFactory) { HighPriority = true };

        /// <summary>
        /// Обновить список борд.
        /// </summary>
        public IOperationViewModel Refresh => refreshCommand;

        private string filter;

        /// <summary>
        /// Фильтр.
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Применить фильтр.
        /// </summary>
        public void ApplyFilter()
        {
            if (cachedResult != null)
            {
                UpdateList(cachedResult, cachedFavorites);
            }
            else
            {
                refreshCommand.Start2(false);
            }
        }

        /// <summary>
        /// Добавить в избранное.
        /// </summary>
        public ICommand AddToFavorites { get; }

        /// <summary>
        /// Убрать из избранного.
        /// </summary>
        public ICommand RemoveFromFavorites { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();

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
                refreshCommand.Start2(false);
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