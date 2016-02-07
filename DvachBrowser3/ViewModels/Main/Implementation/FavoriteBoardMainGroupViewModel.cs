using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группа избранных досок.
    /// </summary>
    public class FavoriteBoardMainGroupViewModel : ViewModelBase, IMainGroupViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FavoriteBoardMainGroupViewModel()
        {
            ((ObservableCollection<IMainTileViewModel>)Tiles).CollectionChanged += (sender, e) =>
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(HasItems));
            };
            AppHelpers.DispatchAction(UpdateInfo);
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => "Избранные доски";

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
                BoardLinkCollection favorites = (await storage.ThreadData.FavoriteThreads.LoadLinkCollectionForReadOnly() as BoardLinkCollection) ?? new BoardLinkCollection() { Links = new List<BoardLinkBase>(), BoardInfo = new Dictionary<string, FavoriteBoardInfo>() };
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
                    Tiles
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