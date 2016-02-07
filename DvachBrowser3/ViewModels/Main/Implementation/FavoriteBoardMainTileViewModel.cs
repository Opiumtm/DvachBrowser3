using System;
using Windows.ApplicationModel.DataTransfer;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тайл главной модели для избранной доски.
    /// </summary>
    public sealed class FavoriteBoardMainTileViewModel : ViewModelBase, IMainTileViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="favorites">Избранные доски.</param>
        public FavoriteBoardMainTileViewModel(BoardLinkBase link, BoardLinkCollection favorites)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            this.link = link;
            string name = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetBoardShortName(link);
            var hash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link);
            if (favorites?.BoardInfo != null)
            {
                if (favorites.BoardInfo.ContainsKey(hash))
                {
                    var bi = favorites.BoardInfo[hash];
                    name = bi.DisplayName;
                }
            }
            TileData = new BoardListBoardDataViewModel(link, name, "Избранные", true, false);
            UpdateId = new ThreadListUpdateId() { LinkHash = hash, SortDate = DateTime.MinValue, Link = link };
        }

        private readonly BoardLinkBase link;

        /// <summary>
        /// Данные тайла.
        /// </summary>
        public object TileData { get; }

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
            ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardPageNavigationTarget(link));
        }

        /// <summary>
        /// Можно удалить.
        /// </summary>
        public bool CanDelete => false;

        /// <summary>
        /// Удалить.
        /// </summary>
        public void Delete()
        {
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
    }
}