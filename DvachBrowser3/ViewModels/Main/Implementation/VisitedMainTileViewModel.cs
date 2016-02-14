using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тайл главной модели для посещённого треда.
    /// </summary>
    public sealed class VisitedMainTileViewModel : ViewModelBase, IMainTileViewModel, IMainTileUpdater
    {
        private readonly BoardLinkBase link;

        private readonly string hash;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="visited">Посещённые.</param>
        /// <param name="favorites">Избранные.</param>
        public VisitedMainTileViewModel(BoardLinkBase link, ThreadLinkCollection visited, ThreadLinkCollection favorites)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            this.link = link;
            TileData = new VisitedThreadTileViewModel(link);
            var upd = (IThreadTileUpdater)TileData;
            upd.UpdateData(visited);
            FavoritesChanged(favorites);
            hash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link);
            if (visited?.ThreadInfo != null)
            {
                threadInfo = visited.ThreadInfo.ContainsKey(hash) ? visited.ThreadInfo[hash] : null;
            }
        }

        /// <summary>
        /// Данные тайла.
        /// </summary>
        public ICommonTileViewModel TileData { get; }

        private ShortThreadInfo threadInfo;

        private bool canAddToFavorites;

        /// <summary>
        /// Можно добавлять в избранные.
        /// </summary>
        public bool CanAddToFavorites
        {
            get { return canAddToFavorites; }
            private set
            {
                canAddToFavorites = value;
                RaisePropertyChanged();
            }
        }

        private bool canRemoveFromFavorites;

        /// <summary>
        /// Можно убрать из избранных.
        /// </summary>
        public bool CanRemoveFromFavorites
        {
            get { return canRemoveFromFavorites; }
            private set
            {
                canRemoveFromFavorites = value;
                RaisePropertyChanged();
            }
        }

        private bool isFavorite;

        /// <summary>
        /// Находится в избранных.
        /// </summary>
        public bool IsFavorite
        {
            get { return isFavorite; }
            private set
            {
                isFavorite = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Добавить в избранное.
        /// </summary>
        public async void AddToFavorites()
        {
            await link.AddToFavoriteThreads(threadInfo);
        }

        /// <summary>
        /// Убрать из избранного.
        /// </summary>
        public async void RemoveFromFavorites()
        {
            await link.RemoveFromFavoriteThreads();
        }

        /// <summary>
        /// Перейти по ссылке с тайла.
        /// </summary>
        public void Navigate()
        {
            ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(link));
        }

        /// <summary>
        /// Можно удалить.
        /// </summary>
        public bool CanDelete => true;

        /// <summary>
        /// Удалить.
        /// </summary>
        public async void Delete()
        {
            await link.RemoveFromVisitedThreads();
        }

        /// <summary>
        /// Можно копировать ссылку.
        /// </summary>
        public bool CanCopyLink => true;

        /// <summary>
        /// Копировать ссылку в клипборд.
        /// </summary>
        public async void CopyLink()
        {
            try
            {
                var webLink = link.GetWebLink();
                if (webLink != null)
                {
                    var dp = new DataPackage();
                    dp.SetText(webLink.ToString());
                    dp.SetWebLink(webLink);
                    Clipboard.SetContent(dp);
                    Clipboard.Flush();
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Идентификатор апдейта.
        /// </summary>
        public ThreadListUpdateId UpdateId => new ThreadListUpdateId()
        {
            LinkHash = hash,
            SortDate = threadInfo?.ViewDate ?? DateTime.MinValue,
            Link = link
        };

        /// <summary>
        /// Избранные изменились.
        /// </summary>
        /// <param name="favorites">Избранные.</param>
        public void FavoritesChanged(ThreadLinkCollection favorites)
        {
            if (favorites?.Links != null)
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var comparer = linkHash.GetComparer();
                IsFavorite = favorites.Links.Any(l => comparer.Equals(l, link));
                CanAddToFavorites = !IsFavorite;
                CanRemoveFromFavorites = IsFavorite;
            }
        }

        /// <summary>
        /// Изменились посещённые.
        /// </summary>
        /// <param name="visited">Посещённые.</param>
        public void VisitedChanged(ThreadLinkCollection visited)
        {
            if (visited?.ThreadInfo != null)
            {
                threadInfo = visited.ThreadInfo.ContainsKey(hash) ? visited.ThreadInfo[hash] : null;
            }
        }
    }
}