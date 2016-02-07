using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группа посещённых тредов.
    /// </summary>
    public sealed class VisitedMainGroupViewModel : ViewModelBase, IMainGroupViewModel, IWeakEventCallback
    {
        public VisitedMainGroupViewModel()
        {
            ((ObservableCollection<IMainTileViewModel>) Tiles).CollectionChanged += (sender, e) =>
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(HasItems));
            };
            ViewModelEvents.VisitedListRefreshed.AddCallback(this);
            ViewModelEvents.FavoritesListRefreshed.AddCallback(this);
            AppHelpers.DispatchAction(UpdateInfo);
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => "Последние";

        /// <summary>
        /// Тайлы.
        /// </summary>
        public IList<IMainTileViewModel> Tiles { get; } = new ObservableCollection<IMainTileViewModel>();

        /// <summary>
        /// Есть элементы.
        /// </summary>
        public bool HasItems => Tiles.Count > 0;

        /// <summary>
        /// Обновить информацию.
        /// </summary>
        public async Task UpdateInfo()
        {
            try
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                ThreadLinkCollection visited = (await storage.ThreadData.VisitedThreads.LoadLinkCollectionForReadOnly() as ThreadLinkCollection) ?? new ThreadLinkCollection() {Links = new List<BoardLinkBase>(), ThreadInfo = new Dictionary<string, ShortThreadInfo>()};
                ThreadLinkCollection favorites = (await storage.ThreadData.FavoriteThreads.LoadLinkCollectionForReadOnly() as ThreadLinkCollection) ?? new ThreadLinkCollection() { Links = new List<BoardLinkBase>(), ThreadInfo = new Dictionary<string, ShortThreadInfo>() };
                var newItems = (visited?.Links ?? new List<BoardLinkBase>()).Select(l =>
                {
                    var hash = linkHash.GetLinkHash(l);
                    if (visited?.ThreadInfo?.ContainsKey(hash) ?? false)
                    {
                        var obj = visited.ThreadInfo[hash];
                        return new ThreadListUpdateId() { LinkHash = hash, SortDate = obj.ViewDate, Link = l};
                    }
                    return new ThreadListUpdateId() { LinkHash = hash, SortDate = DateTime.MinValue, Link = l };
                }).ToArray();
                var updateHelper = new SortedCollectionUpdateHelper<IMainTileViewModel, ThreadListUpdateId, ThreadListUpdateId>(
                    ThreadListUpdateId.EqualityComparer,
                    ThreadListUpdateId.Comparer,
                    a => a,
                    a => a.UpdateId,
                    a => new VisitedMainTileViewModel(a.Link, visited, favorites),
                    (a, b) => true,
                    null,
                    newItems,
                    Tiles
                );
                var update = updateHelper.GetUpdate();
                update.Update();
                foreach (var item in Tiles.OfType<IMainTileUpdater>())
                {
                    item.VisitedChanged(visited);
                    item.FavoritesChanged(favorites);
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == ViewModelEvents.VisitedListRefreshedId || channel?.Id == ViewModelEvents.FavoritesListRefreshedId)
            {
                AppHelpers.DispatchAction(UpdateInfo);
            }
        }
    }
}