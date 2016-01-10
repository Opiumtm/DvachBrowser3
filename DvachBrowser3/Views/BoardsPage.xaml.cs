using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
    public sealed partial class BoardsPage : Page, IPageLifetimeCallback, IShellAppBarProvider, IPageViewModelSource, INavigationRolePage, IWeakEventCallback
    {
        private object lifetimeToken;

        public BoardsPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
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

        private void InitViewModel()
        {
            DataContext = new BoardListViewModel();
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if ("Groups".Equals(e.PropertyName))
                {
                    BoardSource.Source = ViewModel.Groups;
                }
            };
            BoardSource.Source = ViewModel.Groups;
        }

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

        public IBoardListViewModel ViewModel => DataContext as IBoardListViewModel;

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
            syncButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Refresh.CanStart") });
            syncButton.Click += (sender, r) => ViewModel?.Refresh?.Start2(true);
            appBar.PrimaryCommands.Add(syncButton);

            var addButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Add),
                Label = "Добавить"
            };

            addButton.Click += AddButtonOnClick;
            appBar.PrimaryCommands.Add(addButton);

            return appBar;
        }

        private async void AddButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = new AddBoardDialog();
            var r = await dialog.ShowAsync();
            if (r == ContentDialogResult.Primary)
            {
                var model = dialog.ViewModel.GetBoardModel();
                if (model != null)
                {
                    ViewModel.Add(model);
                }
            }
        }

        public void ApplyFilter()
        {
            if (ViewModel == null) return;
            ViewModel.Filter = SearchBox.Text;
            ViewModel.ApplyFilter();
        }

        /// <summary>
        /// Ширина плитки.
        /// </summary>
        public double TileWidth
        {
            get { return (double) GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        /// <summary>
        /// Ширина плитки.
        /// </summary>
        public static readonly DependencyProperty TileWidthProperty = DependencyProperty.Register("TileWidth", typeof (double), typeof (BoardsPage),
            new PropertyMetadata(100.0));

        private void AddToFavorites_OnClick(object sender, RoutedEventArgs e)
        {
            var mf = sender as FrameworkElement;
            var tag = mf?.Tag as IBoardListBoardViewModel;
            ViewModel?.Add(tag);
        }

        private void RemoveFromFavorites_OnClick(object sender, RoutedEventArgs e)
        {
            var mf = sender as FrameworkElement;
            var tag = mf?.Tag as IBoardListBoardViewModel;
            ViewModel?.Remove(tag);
        }

        /// <summary>
        /// Получить модель представления.
        /// </summary>
        /// <returns>Модель представления.</returns>
        public object GetViewModel()
        {
            return ViewModel;
        }

        private void BoardInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var mf = sender as FrameworkElement;
            var tag = mf?.Tag as IBoardListBoardViewModel;
            if (tag != null)
            {
                ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardInfoNavigationTarget(tag.Link));
            }
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.BoardList;

        private void BoardTile_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var mf = sender as BoardTile;
            var tag = mf?.ViewModel;
            if (tag != null)
            {
                ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardPageNavigationTarget(tag.Link));
            }
        }
    }
}
