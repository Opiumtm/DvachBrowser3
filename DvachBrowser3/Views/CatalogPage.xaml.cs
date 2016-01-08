﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using DvachBrowser3.ViewModels;
using Template10.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CatalogPage : Page, IPageLifetimeCallback, INotifyPropertyChanged, IPageViewModelSource, IShellAppBarProvider, INavigationRolePage, IWeakEventCallback
    {
        public CatalogPage()
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

        private void OnDownloadFinished(object sender, OperationProgressFinishedEventArgs operationProgressFinishedEventArgs)
        {
            PostView.ViewModel = null;
        }

        private void OnDownloadStarted(object sender, EventArgs eventArgs)
        {
            PostPreview.IsContentVisible = false;
        }

        public IBoardCatalogViewModel ViewModel => DataContext as IBoardCatalogViewModel;

        public IPostCollectionViewModel ColViewModel => ViewModel;

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
            }
            var vm = new BoardCatalogViewModel(navigatedLink);
            var isBackNavigated = e.NavigationMode == NavigationMode.Back;
            vm.IsBackNavigatedToViewModel = isBackNavigated;
            vm.Update.Progress.Started += OnDownloadStarted;
            vm.Update.Progress.Finished += OnDownloadFinished;
            DataContext = vm;
            OnPropertyChanged(nameof(ViewModel));
            OnPropertyChanged(nameof(ColViewModel));
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

            appBar.PrimaryCommands.Add(syncButton);

            return appBar;
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Catalog;

        /// <summary>
        /// Ширина тайла.
        /// </summary>
        public double TileWidth
        {
            get { return (double) GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        /// <summary>
        /// Ширина тайла.
        /// </summary>
        public static readonly DependencyProperty TileWidthProperty = DependencyProperty.Register("TileWidth", typeof (double), typeof (CatalogPage),
            new PropertyMetadata(100.0));

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

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            PostPreview.IsContentVisible = false;
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
    }
}