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
    /// Тайл главной модели для избранного.
    /// </summary>
    public sealed class FavoriteMainTileViewModel : ViewModelBase, IMainTileViewModel, IMainTileUpdater
    {
        private readonly BoardLinkBase link;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="favorites">Избранные треды.</param>
        public FavoriteMainTileViewModel(BoardLinkBase link, ThreadLinkCollection favorites)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            this.link = link;
            TileData = new FavoriteThreadTileViewModel(link);
            var upd = (IThreadTileUpdater) TileData;
            var si = upd?.UpdateData(favorites);
            if (si != null)
            {
                UpdateId = new ThreadListUpdateId() { LinkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link), SortDate = si.AddedDate, Link = link };
            }
            else
            {
                UpdateId = new ThreadListUpdateId() { LinkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link), SortDate = DateTime.MinValue, Link = link };
            }
        }

        /// <summary>
        /// Данные тайла.
        /// </summary>
        public ICommonTileViewModel TileData { get; }

        /// <summary>
        /// Можно добавлять в избранные.
        /// </summary>
        public bool CanAddToFavorites => false;

        /// <summary>
        /// Можно убрать из избранных.
        /// </summary>
        public bool CanRemoveFromFavorites => false;

        /// <summary>
        /// Находится в избранных.
        /// </summary>
        public bool IsFavorite => true;

        /// <summary>
        /// Добавить в избранное.
        /// </summary>
        public void AddToFavorites()
        {
        }

        /// <summary>
        /// Убрать из избранного.
        /// </summary>
        public void RemoveFromFavorites()
        {
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
            await link.RemoveFromFavoriteThreads();
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
        public ThreadListUpdateId UpdateId { get; }

        /// <summary>
        /// Избранные изменились.
        /// </summary>
        /// <param name="favorites">Избранные.</param>
        public void FavoritesChanged(ThreadLinkCollection favorites)
        {
            (TileData as IThreadTileUpdater)?.UpdateData(favorites);
        }

        /// <summary>
        /// Изменились посещённые.
        /// </summary>
        /// <param name="visited">Посещённые.</param>
        public void VisitedChanged(ThreadLinkCollection visited)
        {
        }
    }
}