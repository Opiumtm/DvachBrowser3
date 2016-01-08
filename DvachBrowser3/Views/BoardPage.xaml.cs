using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views.Partial;
using Template10.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BoardPage : Page, IPageLifetimeCallback, IPageViewModelSource, IShellAppBarProvider, INavigationRolePage, INotifyPropertyChanged, INavigationDataPage, IWeakEventCallback
    {
        public BoardPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            AppEvents.AppResume.AddCallback(this);
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == AppEvents.AppResumeId)
            {
                AppResume?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IBoardPageLoaderViewModel ViewModel => DataContext as IBoardPageLoaderViewModel;

        /// <summary>
        /// Заход на страницу.
        /// </summary>
        public event EventHandler<NavigationEventArgs> NavigatedTo;

        /// <summary>
        /// Уход со страницы.
        /// </summary>
        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        /// <summary>
        /// Восстановление приложения.
        /// </summary>
        public event EventHandler<object> AppResume;

        private BoardLinkBase navigatedLink;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var link = NavigationHelper.GetLinkFromParameter(e.Parameter);
            navigatedLink = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().BoardPageLinkFromBoardLink(link);
            if (navigatedLink == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
            }
            var vm = new BoardPageLoaderViewModel(navigatedLink);
            isBackNavigated = e.NavigationMode == NavigationMode.Back;
            vm.IsBackNavigatedToViewModel = isBackNavigated;
            vm.PageLoaded += BoardOnPageLoaded;
            vm.PageLoadStarted += BoardOnPageLoadStarted;
            DataContext = vm;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(ViewModel));
            NavigatedTo?.Invoke(this, e);
        }

        private void BoardOnPageLoadStarted(object sender, EventArgs eventArgs)
        {
            ThreadPreviewPopup.IsContentVisible = false;
        }

        private void BoardOnPageLoaded(object sender, EventArgs eventArgs)
        {
            ThreadPreview.ViewModel = null;
            if (savedTopThreadHash != null)
            {
                var savedHash = savedTopThreadHash;
                savedTopThreadHash = null;
                AppHelpers.DispatchAction(() =>
                {
                    var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                    var el = ViewModel?.Page?.Threads?.FirstOrDefault(t =>
                    {
                        var o = t?.OpPost?.Link;
                        if (o == null)
                        {
                            return false;
                        }
                        return linkHash.GetLinkHash(o) == savedHash;
                    });
                    if (el != null)
                    {
                        MainList.ScrollIntoView(el);
                    }
                });
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        /// <summary>
        /// Получить модель представления.
        /// </summary>
        /// <returns>Модель представления.</returns>
        public object GetViewModel()
        {
            return ViewModel;
        }

        /// <summary>
        /// Получить нижнюю строку команд.
        /// </summary>
        /// <returns>Строка команд.</returns>
        public AppBar GetBottomAppBar()
        {
            var appBar = new CommandBar();

            var syncButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Sync),
                Label = "Обновить"
            };
            syncButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Update.CanStart") });
            syncButton.Click += (sender, r) => ViewModel?.Update?.Start2(BoardPageLoaderUpdateMode.Load);

            var nextButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Forward),
                Label = "След.стр."
            };
            nextButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Page.CanGoNextPage"), FallbackValue = false });
            nextButton.Click += (sender, r) =>
            {
                var p = ViewModel?.Page?.NextPageLink;
                if (p != null)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardPageNavigationTarget(p));
                }
            };

            var prevButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Back),
                Label = "Пред.стр."
            };
            prevButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Page.CanGoPrevPage"), FallbackValue = false });
            prevButton.Click += (sender, r) =>
            {
                var p = ViewModel?.Page?.PrevPageLink;
                if (p != null)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardPageNavigationTarget(p));
                }
            };

            var gotoPage = new AppBarButton()
            {
                Label = "Перейти к странице"
            };

            gotoPage.Click += async (sender, r) =>
            {
                try
                {
                    var pl = ViewModel?.Page?.GetPages() ?? new int[] { 0 };
                    var dialog = new SelectPageDialog()
                    {
                        MinPage = pl.DefaultIfEmpty(0).Min(),
                        MaxPage = pl.DefaultIfEmpty(0).Max(),
                    };
                    var dr = await dialog.ShowAsync();
                    if (dr == ContentDialogResult.Primary && dialog.SelectedPage != null && ViewModel?.PageLink != null)
                    {
                        var l = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().SetBoardPage(ViewModel?.PageLink, dialog.SelectedPage ?? 0);
                        if (l != null)
                        {
                            ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardPageNavigationTarget(l));
                        }
                    }
                }
                catch (Exception ex)
                {
                    await AppHelpers.ShowError(ex);
                }
            };

            var catalog = new AppBarButton()
            {
                Label = "Каталог"
            };            
            catalog.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.CanInvokeCatalog"), Mode = BindingMode.OneWay, FallbackValue = false });

            catalog.Click += (sender, e) =>
            {
                var p = ViewModel?.PageLink;
                if (p != null)
                {
                    var catLink = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetCatalogLinkFromAnyLink(p, BoardCatalogSort.Bump);
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardCatalogNavigationTarget(catLink));
                }
            };

            appBar.PrimaryCommands.Add(prevButton);
            appBar.PrimaryCommands.Add(nextButton);
            appBar.PrimaryCommands.Add(syncButton);

            appBar.SecondaryCommands.Add(gotoPage);
            appBar.SecondaryCommands.Add(catalog);

            return appBar;
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.BoardPage;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Получить данные навигации.
        /// </summary>
        /// <returns>Данные навигации.</returns>
        public Task<Dictionary<string, object>> GetNavigationData()
        {
            var element = MainList.GetTopViewIndex();
            var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
            if (element?.OpPost?.Link != null)
            {
                var hash = linkHash.GetLinkHash(element.OpPost.Link);
                return Task.FromResult(new Dictionary<string, object>()
                {
                    { "TopVisibleThread", hash }
                });
            }
            return Task.FromResult(new Dictionary<string, object>()
            {
            });
        }

        private string savedTopThreadHash;

        private bool isBackNavigated;

        /// <summary>
        /// Восстановить данные навигации.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат.</returns>
        public Task RestoreNavigationData(Dictionary<string, object> data)
        {
            if (!isBackNavigated)
            {
                return Task.FromResult(true);
            }
            if (data != null && data.ContainsKey("TopVisibleThread"))
            {
                savedTopThreadHash = data["TopVisibleThread"] as string;
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Ключ навигации.
        /// </summary>
        public string NavigationDataKey
        {
            get
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                return $"{this.GetType().FullName}::{linkHash.GetLinkHash(navigatedLink)}";
            }
        }

        private void MainList_OnThreadTapped(object sender, ThreadPreviewTappedEventArgs e)
        {
            if (e?.Thread != null && !ViewModel.Update.Progress.IsActive)
            {
                ThreadPreview.ViewModel = e.Thread;
                ThreadPreviewPopup.IsContentVisible = true;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back && ThreadPreviewPopup.IsContentVisible)
            {
                ThreadPreviewPopup.IsContentVisible = false;
                e.Cancel = true;
                return;
            }
            base.OnNavigatingFrom(e);
        }
    }
}
