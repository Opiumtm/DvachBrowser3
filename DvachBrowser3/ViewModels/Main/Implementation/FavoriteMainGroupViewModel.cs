using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using DvachBrowser3.Styles;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группа избранных тредов.
    /// </summary>
    public sealed class FavoriteMainGroupViewModel : ObservableCollection<IMainTileViewModel>, IMainGroupViewModel, IWeakEventCallback
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FavoriteMainGroupViewModel()
        {
            ViewModelEvents.VisitedListRefreshed.AddCallback(this);
            ViewModelEvents.FavoritesListRefreshed.AddCallback(this);
            AppHelpers.DispatchAction(UpdateInfo, false, 0);
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => "Избранные";

        private readonly Lazy<IStyleManager> styleManager = new Lazy<IStyleManager>(() => StyleManagerFactory.Current.GetManager());

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => styleManager.Value;

        /// <summary>
        /// Обновить информацию.
        /// </summary>
        public async Task UpdateInfo()
        {
            try
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                ThreadLinkCollection visited = (await storage.ThreadData.VisitedThreads.LoadLinkCollectionForReadOnly() as ThreadLinkCollection) ?? new ThreadLinkCollection() { Links = new List<BoardLinkBase>(), ThreadInfo = new Dictionary<string, ShortThreadInfo>() };
                ThreadLinkCollection favorites = (await storage.ThreadData.FavoriteThreads.LoadLinkCollectionForReadOnly() as ThreadLinkCollection) ?? new ThreadLinkCollection() { Links = new List<BoardLinkBase>(), ThreadInfo = new Dictionary<string, ShortThreadInfo>() };
                var newItems = (favorites?.Links ?? new List<BoardLinkBase>()).Select(l =>
                {
                    var hash = linkHash.GetLinkHash(l);
                    if (visited?.ThreadInfo?.ContainsKey(hash) ?? false)
                    {
                        var obj = favorites.ThreadInfo[hash];
                        return new ThreadListUpdateId() { LinkHash = hash, SortDate = obj.AddedDate, Link = l };
                    }
                    return new ThreadListUpdateId() { LinkHash = hash, SortDate = DateTime.MinValue, Link = l };
                }).OrderBy(l => l, ThreadListUpdateId.Comparer).ToArray();
                var updateHelper = new SortedCollectionUpdateHelper<IMainTileViewModel, ThreadListUpdateId, ThreadListUpdateId>(
                    ThreadListUpdateId.EqualityComparer,
                    ThreadListUpdateId.Comparer,
                    a => a,
                    a => a.UpdateId,
                    a => new FavoriteMainTileViewModel(a.Link, favorites), 
                    (a, b) => true,
                    null,
                    newItems,
                    this
                );
                var update = updateHelper.GetUpdate();
                update.Update();
                foreach (var item in this.OfType<IMainTileUpdater>())
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
                AppHelpers.DispatchAction(UpdateInfo, false, 0);
            }
        }
    }
}