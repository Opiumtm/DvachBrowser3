using System;
using System.Threading.Tasks;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
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
            update = new StdEngineOperationWrapper<IBoardPageLoaderResult>(OperationFactory);
            update.Finished += UpdateOnFinished;
            update.ResultGot += UpdateOnResultGot;
        }

        private void UpdateOnResultGot(object sender, EventArgs e)
        {
            var result = update.Result;
#pragma warning disable 4014
            BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
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

        private async void UpdateOnFinished(object sender, OperationProgressFinishedEventArgs e)
        {
            if (e.Error != null && !e.IsCancelled)
            {
                await BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    await AppHelpers.ShowError(e.Error);
                });
            }
        }

        private IEngineOperationsWithProgress<IBoardPageLoaderResult, EngineProgress> OperationFactory(object o)
        {
            var updateMode = BoardPageLoaderUpdateMode.Load;
            if (o != null)
            {
                if (o.GetType() == typeof (BoardPageLoaderUpdateMode))
                {
                    updateMode = (BoardPageLoaderUpdateMode) o;
                }
            }
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
            var profile = ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>().CurrentProfile;
            if (profile.UpdateBoardPageOnEntry)
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
    }
}