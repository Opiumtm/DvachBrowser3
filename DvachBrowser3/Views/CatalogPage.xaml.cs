﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views.Partial;
using Template10.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CatalogPage : Page, IPageLifetimeCallback, INotifyPropertyChanged, IPageViewModelSource, IShellAppBarProvider, INavigationRolePage, IWeakEventCallback, IStyleManagerFactory
    {
        private object lifetimeToken;

        public CatalogPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            this.DataContext = this;
            lifetimeToken = this.BindAppLifetimeEvents();
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Unloaded -= OnUnloaded;
            Bindings.StopTracking();
            DataContext = null;
            ViewModel = null;
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

        private void OnDownloadFinished(object sender, OperationProgressFinishedEventArgs operationProgressFinishedEventArgs)
        {
            PostView.ViewModel = null;
        }

        private void OnDownloadStarted(object sender, EventArgs eventArgs)
        {
            PostPreview.IsContentVisible = false;
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IBoardCatalogViewModel ViewModel
        {
            get { return (IBoardCatalogViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IBoardCatalogViewModel), typeof (CatalogPage), new PropertyMetadata(null, ViewModelPropertyChangedCallback));

        private static void ViewModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as CatalogPage;
            if (obj != null)
            {
                obj.ColViewModel = e.NewValue as IPostCollectionViewModel;
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostCollectionViewModel ColViewModel
        {
            get { return (IPostCollectionViewModel) GetValue(ColViewModelProperty); }
            set { SetValue(ColViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ColViewModelProperty = DependencyProperty.Register("ColViewModel", typeof (IPostCollectionViewModel), typeof (CatalogPage), new PropertyMetadata(null));


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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var link = NavigationHelper.GetLinkFromParameter(e.Parameter);
            var navigatedLink = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetCatalogLinkFromAnyLink(link);
            if (navigatedLink == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            var vm = new BoardCatalogViewModel(navigatedLink);
            var isBackNavigated = e.NavigationMode == NavigationMode.Back;
            vm.IsBackNavigatedToViewModel = isBackNavigated;
            vm.Update.Progress.Started += LiteWeakEventHelper.CreateHandler(new WeakReference<CatalogPage>(this), (root, esender, ee) => root.OnDownloadStarted(esender, ee));
            vm.Update.Progress.Finished += LiteWeakEventHelper.CreateProgressFinishedHandler(new WeakReference<CatalogPage>(this), (root, esender, ee) => root.OnDownloadFinished(esender, ee));
            ViewModel = vm;
            FilteredPosts = vm;
            NavigatedTo?.Invoke(this, e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            syncButton.Click += (sender, r) => ViewModel?.Update?.Start2(BoardCatalogUpdateMode.Load);

            return appBar;
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Catalog;

        private void CatalogElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag as IPostViewModel;
            if (t != null && !ViewModel.Update.Progress.IsActive)
            {
                PostView.ViewModel = t;
                PostPreviewScroll.ChangeView(null, 0.0, null);
                PostPreview.IsContentVisible = true;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back && PostPreview.IsContentVisible)
            {
                e.Cancel = true;
                PostPreview.IsContentVisible = false;
                return;
            }
            base.OnNavigatingFrom(e);
        }

        private void PostView_OnShowFullThread(object sender, ShowFullThreadEventArgs e)
        {
            var tlink = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetThreadLinkFromAnyLink(e.Post?.Link);
            if (tlink != null)
            {
                ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(tlink));
            }
        }

        /// <summary>
        /// Фильтрованные посты.
        /// </summary>
        public IPostCollectionViewModel FilteredPosts
        {
            get { return (IPostCollectionViewModel) GetValue(FilteredPostsProperty); }
            set { SetValue(FilteredPostsProperty, value); }
        }

        /// <summary>
        /// Фильтрованные посты.
        /// </summary>
        public static readonly DependencyProperty FilteredPostsProperty = DependencyProperty.Register("FilteredPosts", typeof (IPostCollectionViewModel), typeof (CatalogPage), new PropertyMetadata(null));


        private void SetFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                FilteredPosts = ViewModel;
            }
            else
            {
                FilteredPosts = ViewModel.FilterPosts(new TextPostCollectionSearchQuery(filter), false);
            }
        }

        private void FilterBox_OnFilterUpdated(object sender, EventArgs e)
        {
            SetFilter(FilterBox.FilterSting);
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            FilterBox.AnimatedVisibility = Visibility.Visible;
        }

        private void CatalogPage_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                e.Handled = true;
                PostPreview.IsContentVisible = false;
            }
        }

        private void MainList_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var tile = GetTileImage(args);
            if (tile != null)
            {
                tile.Opacity = 0;
                tile.LoadingSuspended = true;
                args.RegisterUpdateCallback(Phase1Callback);
            }
        }

        private void Phase1Callback(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var tile = GetTileImage(args);
            if (tile != null)
            {
                tile.Opacity = 1;
                tile.LoadingSuspended = false;
            }
        }

        private TileImage GetTileImage(ContainerContentChangingEventArgs args)
        {
            var contentRoot = args.ItemContainer.ContentTemplateRoot as Grid;
            if (contentRoot != null)
            {
                foreach (var c in contentRoot.Children)
                {
                    if (c is TileImage)
                    {
                        return c as TileImage;
                    }
                }
            }
            return null;
        }

        private Lazy<IStyleManager> _styleManager = new Lazy<IStyleManager>(() => new StyleManager());
        IStyleManager IStyleManagerFactory.GetManager()
        {
            return _styleManager.Value;
        }
    }
}
