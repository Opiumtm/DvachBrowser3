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
using Template10.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThreadPage : Page, IPageLifetimeCallback, IPageViewModelSource, IShellAppBarProvider, INavigationRolePage, INotifyPropertyChanged, INavigationDataPage, IWeakEventCallback
    {
        public ThreadPage()
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

        private BoardLinkBase navigatedLink;

        private BoardLinkBase navigatePostLink;

        private string savedTopPostHash;

        private bool isBackNavigated;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            var link = NavigationHelper.GetLinkFromParameter(e.Parameter);
            navigatedLink = linkTransform.GetThreadLinkFromAnyLink(link);
            navigatePostLink = linkTransform.GetPostLinkFromAnyLink(link);
            if (navigatedLink == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
            }
            var vm = new ThreadViewModel(navigatedLink);
            vm.PostsUpdated += OnPostsUpdated;
            isBackNavigated = e.NavigationMode == NavigationMode.Back;
            vm.IsBackNavigatedToViewModel = isBackNavigated;
            DataContext = vm;
            OnPropertyChanged(nameof(ViewModel));
            NavigatedTo?.Invoke(this, e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        private void OnPostsUpdated(object sender, EventArgs eventArgs)
        {
            AppHelpers.ActionOnUiThread(() =>
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var navigatedPostLinkVal = navigatePostLink;
                var saveTopHashVal = savedTopPostHash;
                this.navigatePostLink = null;
                this.savedTopPostHash = null;
                if (navigatedPostLinkVal != null || saveTopHashVal != null)
                {
                    string checkHash;
                    if (navigatedPostLinkVal != null)
                    {
                        checkHash = linkHash.GetLinkHash(navigatedPostLinkVal);
                    }
                    else
                    {
                        checkHash = saveTopHashVal;
                    }
                    var el = ViewModel.Posts.FirstOrDefault(t =>
                    {
                        var o = t?.Link;
                        if (o == null)
                        {
                            return false;
                        }
                        return linkHash.GetLinkHash(o) == checkHash;
                    });
                    if (el != null)
                    {
                        ScrollIntoView(el);
                    }
                }
            });
        }

        private void ScrollIntoView(IPostViewModel post)
        {
            // todo: написать скроллинг, когда будет готова разметка
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

            var syncButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Sync),
                Label = "Обновить"
            };
            syncButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Update.CanStart") });
            syncButton.Click += (sender, e) => ViewModel?.Synchronize();

            appBar.PrimaryCommands.Add(syncButton);

            return appBar;
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Thread;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получить данные навигации.
        /// </summary>
        /// <returns>Данные навигации.</returns>
        public Task<Dictionary<string, object>> GetNavigationData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Восстановить данные навигации.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат.</returns>
        public Task RestoreNavigationData(Dictionary<string, object> data)
        {
            if (data != null && data.ContainsKey("TopVisiblePost"))
            {
                savedTopPostHash = data["TopVisiblePost"] as string;
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

        private void OnPropertyChanged(string propertyName)
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
        /// Модель представления.
        /// </summary>
        public IThreadViewModel ViewModel => DataContext as IThreadViewModel;

        /// <summary>
        /// Коллекция постов.
        /// </summary>
        public IPostCollectionViewModel PostCollection => DataContext as IPostCollectionViewModel;
    }
}
