using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using DvachBrowser3.Styles;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группа избранных досок.
    /// </summary>
    public class FavoriteBoardMainGroupViewModel : ObservableCollection<IMainTileViewModel>, IMainGroupViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FavoriteBoardMainGroupViewModel()
        {
            AppHelpers.DispatchAction(UpdateInfo);
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => "Избранные доски";

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
                BoardLinkCollection favorites = (await storage.ThreadData.FavoriteBoards.LoadLinkCollectionForReadOnly() as BoardLinkCollection) ?? new BoardLinkCollection() { Links = new List<BoardLinkBase>(), BoardInfo = new Dictionary<string, FavoriteBoardInfo>() };
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                var updateHelper = new SortedCollectionUpdateHelper<IMainTileViewModel, BoardLinkBase, BoardLinkBase>(
                    linkHash.GetComparer(),
                    linkTransform.GetLinkComparer(),
                    a => a,
                    a => a.UpdateId.Link,
                    a => new FavoriteBoardMainTileViewModel(a, favorites),
                    (a, b) => true,
                    null,
                    favorites.Links ?? new List<BoardLinkBase>(),
                    this
                );
                var update = updateHelper.GetUpdate();
                update.Update();
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }
    }
}