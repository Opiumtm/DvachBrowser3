﻿using System;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Links;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;
using Template10.Common;
using XamlAnimatedGif;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaPage : Page, IWeakEventCallback, IPageLifetimeCallback, IShellAppBarProvider, IPageViewModelSource, INavigationRolePage, INotifyPropertyChanged
    {
        private object lifetimeToken;

        public MediaPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            lifetimeToken = this.BindAppLifetimeEvents();
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var link = NavigationHelper.GetLinkFromParameter(e.Parameter);
            if (link == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            if ((link.LinkKind & BoardLinkKind.Media) == 0)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            var vm = new BigMediaSourceViewModel(link) { SetImageSource = false };
            vm.Load.Progress.Finished += ProgressOnFinished;
            vm.Load.Progress.Started += ProgressOnStarted;
            vm.ImageSourceGot += ViewModelOnImageSourceGot;
            DataContext = vm;
            OnPropertyChanged(nameof(ViewModel));
            OnPropertyChanged(nameof(ImageViewModel));
            NavigatedTo?.Invoke(this, e);
        }

        private void ProgressOnStarted(object sender, EventArgs eventArgs)
        {
            AppHelpers.ActionOnUiThread(() =>
            {
                ViewKind = PageView.Loading;
                IsLoaded = false;
                return Task.FromResult(true);
            });
        }

        private void ViewModelOnImageSourceGot(object sender, ImageSourceGotEventArgs e)
        {
            AppHelpers.ActionOnUiThread(() =>
            {
                bool isSet = false;
                switch (ViewModel.SourceType)
                {
                    case BigMediaSourceType.Gif:
                        if (e.CacheUri != null)
                        {
                            AnimationBehavior.SetRepeatBehavior(MainGifImage, RepeatBehavior.Forever);
                            AnimationBehavior.SetAutoStart(MainGifImage, true);
                            AnimationBehavior.SetSourceUri(MainGifImage, e.CacheUri);
                            MainGifImage.Visibility = Visibility.Visible;
                            MainImage.Visibility = Visibility.Collapsed;
                            isSet = true;
                        }
                        break;
                    case BigMediaSourceType.Static:
                        if (e.CacheUri != null)
                        {
                            MainImage.Source = new BitmapImage(e.CacheUri);
                            MainGifImage.Visibility = Visibility.Collapsed;
                            MainImage.Visibility = Visibility.Visible;
                            isSet = true;
                        }
                        break;
                }
                if (!isSet)
                {
                    MainGifImage.Visibility = Visibility.Collapsed;
                    MainImage.Visibility = Visibility.Collapsed;
                    ViewKind = PageView.OpenInBrowser;
                }
                else
                {
                    ViewKind = PageView.Image;
                }
                IsLoaded = true;
                return Task.FromResult(true);
            });
        }

        private void ProgressOnFinished(object sender, OperationProgressFinishedEventArgs e)
        {
            AppHelpers.ActionOnUiThread(async () =>
            {
                if (e.Error != null)
                {
                    await AppHelpers.ShowError(e.Error);
                }
            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

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

            var saveButton = new AppBarButton()
            {
                Label = "Сохранить",
                Icon = new SymbolIcon(Symbol.SaveLocal)
            };            
            saveButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("IsLoaded"), Mode = BindingMode.OneWay});
            saveButton.Click += SaveButtonOnClick;

            var webButton = new AppBarButton()
            {
                Label = "Открыть в web",
                Icon = new SymbolIcon(Symbol.Globe)
            };
            webButton.Click += WebButtonOnClick;

            var appButton = new AppBarButton()
            {
                Label = "В программе",
                Icon = new SymbolIcon(Symbol.SlideShow)
            };
            appButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("IsLoaded"), Mode = BindingMode.OneWay });
            appButton.Click += AppButtonOnClick;

            appBar.PrimaryCommands.Add(webButton);
            appBar.PrimaryCommands.Add(appButton);
            appBar.PrimaryCommands.Add(saveButton);

            return appBar;
        }

        private async void AppButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.WithHandleErrors(async () =>
            {
                await ViewModel.OpenInProgram();
            }, true);
        }

        private async void WebButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.WithHandleErrors(async () =>
            {
                await ViewModel.OpenInBrowser();
            }, true);
        }

        private async void SaveButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.WithHandleErrors(async () =>
            {
                await ViewModel.SaveToFile();
            }, true);
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
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.ImageView;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IBigMediaSourceViewModel ViewModel => DataContext as IBigMediaSourceViewModel;

        /// <summary>
        /// Модель изображения.
        /// </summary>
        public IImageSourceViewModel ImageViewModel => DataContext as IImageSourceViewModel;

        private PageView viewKind;

        private PageView ViewKind
        {
            get { return viewKind; }
            set
            {
                viewKind = value;
                OnPropertyChanged(nameof(ViewKind));
                switch (value)
                {
                    case PageView.Image:
                        ImageViewer.Visibility = Visibility.Visible;
                        LoadingViewer.Visibility = Visibility.Collapsed;
                        OpenInBrowserViewer.Visibility = Visibility.Collapsed;
                        break;
                    case PageView.OpenInBrowser:
                        ImageViewer.Visibility = Visibility.Collapsed;
                        LoadingViewer.Visibility = Visibility.Collapsed;
                        OpenInBrowserViewer.Visibility = Visibility.Visible;
                        break;
                    default:
                        ImageViewer.Visibility = Visibility.Collapsed;
                        LoadingViewer.Visibility = Visibility.Visible;
                        OpenInBrowserViewer.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private enum PageView
        {
            Loading,
            Image,
            OpenInBrowser
        }

        public IStyleManager StyleManager => Shell.StyleManager;

        private bool isLoaded;

        public bool IsLoaded
        {
            get { return isLoaded; }
            private set
            {
                isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
    }
}