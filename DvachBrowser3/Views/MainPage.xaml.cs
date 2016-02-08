using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;
using Template10.Common;

namespace DvachBrowser3.Views
{
    public sealed partial class MainPage : Page, IPageLifetimeCallback, IShellAppBarProvider, INavigationRolePage, IWeakEventCallback, IPageViewModelSource
    {
        private object lifetimeToken;

        public MainPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            InitializeComponent();
            lifetimeToken = this.BindAppLifetimeEvents();
            InitViewModel();
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

        public event EventHandler<NavigationEventArgs> NavigatedTo;

        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        public event EventHandler<object> AppResume;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigatedTo?.Invoke(this, e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        public AppBar GetBottomAppBar()
        {
            var appBar = new CommandBar();
            
            var syncButton = new AppBarButton()
            {
                Label = "Обновить",
                Icon = new SymbolIcon(Symbol.Sync),                
            };            
            syncButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = ViewModel, Path = new PropertyPath("CheckForUpdates.CanStart"), Mode = BindingMode.OneWay });
            syncButton.Click += (sender, e) => ViewModel.CheckForUpdates.Start();

            appBar.PrimaryCommands.Add(syncButton);

            return appBar;
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Main;

        public IMainViewModel ViewModel => DataContext as IMainViewModel;

        /// <summary>
        /// Получить модель представления.
        /// </summary>
        /// <returns>Модель представления.</returns>
        public object GetViewModel()
        {
            return ViewModel;
        }

        public IStyleManager StyleManager => Shell.StyleManager;

        private void InitViewModel()
        {
            DataContext = new MainViewModel();
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if ("Groups".Equals(e.PropertyName))
                {
                    MainSource.Source = ViewModel.Groups;
                }
            };
            MainSource.Source = ViewModel.Groups;
        }

        private void DeleteItem_OnClick(object sender, RoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag as IMainTileViewModel;
            t?.Delete();
        }

        private void AddToFavoritesItem_OnClick(object sender, RoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag as IMainTileViewModel;
            t?.AddToFavorites();
        }

        private void RemoveFromFavoritesItem_OnClick(object sender, RoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag as IMainTileViewModel;
            t?.RemoveFromFavorites();
        }

        private void CopyLinkItem_OnClick(object sender, RoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag as IMainTileViewModel;
            t?.CopyLink();
        }

        private void Tile_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag as IMainTileViewModel;
            t?.Navigate();
        }
    }

    public class MainTileTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoardTemplate { get; set; }

        public DataTemplate ThreadTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var itemObj = item as IMainTileViewModel;
            if (itemObj?.TileData != null)
            {
                var item2 = itemObj.TileData;
                if (item2 is IThreadTileViewModel)
                {
                    return ThreadTemplate;
                }
                if (item2 is IBoardListBoardViewModel)
                {
                    return BoardTemplate;
                }
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
