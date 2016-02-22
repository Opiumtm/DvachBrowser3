using System;
using System.Threading.Tasks;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
using Template10.Common;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Загрузчик страницы борды.
    /// </summary>
    public sealed class BoardPageLoaderViewModel : ViewModelBase, IBoardPageLoaderViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pageLink">Ссылка на страницу.</param>
        public BoardPageLoaderViewModel(BoardLinkBase pageLink)
        {
            if (pageLink == null) throw new ArgumentNullException(nameof(pageLink));
            PageLink = pageLink;
            update = new StdEngineOperationWrapper<IBoardPageLoaderResult>(OperationFactory) { HighPriority = true };
            update.Finished += UpdateOnFinished;
            update.ResultGot += UpdateOnResultGot;
            update.Started += UpdateOnStarted;
            PageNum = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetBoardPage(pageLink);
            AppHelpers.DispatchAction(async () =>
            {
                var t = await BoardTitleHelper.GetBoardTitle(pageLink) ?? "";
                Title = t;
                TitleWithPage = $"[{PageNum}] {t}";
            });
            var engine = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>().FindEngine(pageLink.Engine);
            if (engine != null)
            {
                CanInvokeCatalog = (engine.Capability & EngineCapability.Catalog) != 0;
            }
            else
            {
                CanInvokeCatalog = false;
            }
        }

        private void UpdateOnStarted(object sender, EventArgs eventArgs)
        {
            PageLoadStarted?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateOnResultGot(object sender, EventArgs e)
        {
            var result = update.Result;
#pragma warning disable 4014
            AppHelpers.DispatchAction(async () =>
#pragma warning restore 4014
            {
                try
                {
                    if (result != null)
                    {
                        if (result.Data != null)
                        {
                            var viewModel = new BoardPageViewModel(result.Data);
                            await viewModel.Initialize();
                            Page?.Banner?.Cancel();
                            Page = viewModel;
                            PageLoaded?.Invoke(this, EventArgs.Empty);
                        }
                        if (result.IsUpdated)
                        {
                            Page?.TriggerObsolete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    await AppHelpers.ShowError(ex);
                }
            });
        }

        private void UpdateOnFinished(object sender, OperationProgressFinishedEventArgs e)
        {
            if (e.Error != null && !e.IsCancelled)
            {
                AppHelpers.DispatchAction(async () =>
                {
                    if (Page == null && ModeFromArgument(e.Argument) == BoardPageLoaderUpdateMode.Load)
                    {
                        Update.Start2(BoardPageLoaderUpdateMode.GetFromCache);
                    }
                    await AppHelpers.ShowError(e.Error);
                });
            }
        }

        private BoardPageLoaderUpdateMode ModeFromArgument(object o)
        {
            var updateMode = BoardPageLoaderUpdateMode.Load;
            if (o != null)
            {
                if (o.GetType() == typeof(BoardPageLoaderUpdateMode))
                {
                    updateMode = (BoardPageLoaderUpdateMode)o;
                }
            }
            return updateMode;
        }

        private IEngineOperationsWithProgress<IBoardPageLoaderResult, EngineProgress> OperationFactory(object o)
        {
            var updateMode = ModeFromArgument(o);
            return new BoardPageLoaderOperation(ServiceLocator.Current, new BoardPageLoaderArgument()
            {
                PageLink = PageLink,
                UpdateMode = updateMode
            });
        }

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Start()
        {
            var profile = NetworkProfileHelper.CurrentProfile;
            if (profile.UpdateBoardPageOnEntry && !IsBackNavigatedToViewModel)
            {
                Update.Start2(BoardPageLoaderUpdateMode.Load);
            }
            else
            {
                Update.Start2(BoardPageLoaderUpdateMode.GetFromCache);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Stop()
        {
            Update.Cancel();
            Page?.Banner?.Cancel();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск.</returns>
        public Task Resume()
        {
            if (Page == null)
            {
                Update.Start2(BoardPageLoaderUpdateMode.GetFromCache);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        public BoardLinkBase PageLink { get; }

        private IBoardPageViewModel page;

        /// <summary>
        /// Страница.
        /// </summary>
        public IBoardPageViewModel Page
        {
            get { return page; }
            private set
            {
                page = value;
                RaisePropertyChanged();
            }
        }

        private readonly StdEngineOperationWrapper<IBoardPageLoaderResult> update;

        /// <summary>
        /// Операция обновления.
        /// </summary>
        public IOperationViewModel Update => update;

        /// <summary>
        /// Загрузить.
        /// </summary>
        public void Reload()
        {
            if (Update.CanStart)
            {
                Update.Start2(BoardPageLoaderUpdateMode.Load);
            }
        }

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        public void CheckForUpdates()
        {
            if (Update.CanStart)
            {
                Update.Start2(BoardPageLoaderUpdateMode.CheckForUpdates);
            }
        }

        /// <summary>
        /// Страница загружена.
        /// </summary>
        public event EventHandler PageLoaded;

        /// <summary>
        /// Загрузка начата.
        /// </summary>
        public event EventHandler PageLoadStarted;

        private string title;

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title
        {
            get { return title; }
            private set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        public int PageNum { get; }

        private string titleWithPage;

        /// <summary>
        /// Заголовок со страницей.
        /// </summary>
        public string TitleWithPage
        {
            get { return titleWithPage; }
            private set
            {
                titleWithPage = value;
                RaisePropertyChanged();
            }
        }

        private bool isBackNavigatedToViewModel;

        /// <summary>
        /// Была навигация назад.
        /// </summary>
        public bool IsBackNavigatedToViewModel
        {
            get { return isBackNavigatedToViewModel; }
            set
            {
                isBackNavigatedToViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Можно вызывать каталог.
        /// </summary>
        public bool CanInvokeCatalog { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = new StyleManager();
    }
}