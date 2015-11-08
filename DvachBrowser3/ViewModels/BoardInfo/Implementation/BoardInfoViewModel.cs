using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления информации о борде.
    /// </summary>
    public sealed class BoardInfoViewModel : ViewModelBase, IBoardInfoViewModel, IStartableViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardInfoViewModel(BoardLinkBase link)
        {
            this.Link = link;
        }

        private bool isLoading;

        /// <summary>
        /// Информация загружается.
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            private set
            {
                isLoading = value;
                RaisePropertyChanged();
            }
        }

        private bool noInfoAvailable;

        /// <summary>
        /// Нет информации.
        /// </summary>
        public bool NoInfoAvailable
        {
            get { return noInfoAvailable; }
            private set
            {
                noInfoAvailable = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        private bool inFavorites;

        /// <summary>
        /// В избранном.
        /// </summary>
        public bool InFavorites
        {
            get { return inFavorites; }
            private set
            {
                inFavorites = value;
                RaisePropertyChanged();
            }
        }

        private ISummaryBoardInfo boardInfo;

        /// <summary>
        /// Информация о борде.
        /// </summary>
        public ISummaryBoardInfo BoardInfo
        {
            get { return boardInfo; }
            private set
            {
                boardInfo = value;
                RaisePropertyChanged();
            }
        }

        private Action cancelAction;

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Start()
        {
            var isCancelled = false;
            try
            {
                if (Link == null || Link.Engine == null)
                {
                    NoInfoAvailable = true;
                    return;
                }
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var hashService = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var transformService = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                var logic = ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>();
                var comparer = hashService.GetComparer();
                if (!engines.ListEngines().Any(e => StringComparer.OrdinalIgnoreCase.Equals(e, Link.Engine)))
                {
                    NoInfoAvailable = true;
                    return;
                }
                var engine = engines.GetEngineById(Link.Engine);
                IsLoading = true;
                try
                {
                    var favs = await store.ThreadData.FavoriteBoards.LoadLinkCollection();
                    if (favs?.Links != null)
                    {
                        InFavorites = favs.Links.Any(l => comparer.Equals(l, Link));
                    }
                    BoardReference reference = null;
                    if ((engine.Capability & EngineCapability.BoardsListRequest) != 0)
                    {
                        var refsOperation = logic.GetBoardReferences(engine.RootLink, false);
                        using (var tokenSource = new CancellationTokenSource())
                        {
                            cancelAction = () =>
                            {
                                isCancelled = true;
                                // ReSharper disable once AccessToDisposedClosure
                                tokenSource.Cancel();
                            };
                            try
                            {
                                var refs = await refsOperation.Complete(tokenSource.Token);
                                if (refs?.References != null)
                                {
                                    reference = refs.References.Where(r => r?.Link != null).FirstOrDefault(r => comparer.Equals(r.Link, Link));
                                }
                            }
                            finally
                            {
                                cancelAction = null;
                            }
                        }
                    }
                    if (reference == null)
                    {
                        reference = engine.GetDefaultBoardData("Без категории", transformService.GetBoardShortName(Link));
                    }
                    if (reference != null)
                    {
                        BoardInfo = new SummaryBoardInfo(reference);
                    }
                    else
                    {
                        NoInfoAvailable = true;
                    }
                }
                finally
                {
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                if (!isCancelled)
                {
                    await AppHelpers.ShowError(ex);
                }
            }
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Stop()
        {
            cancelAction?.Invoke();
            return Task.FromResult(true);
        }
    }
}