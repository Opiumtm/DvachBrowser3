using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
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
using DvachBrowser3.Storage;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views.Partial;
using Template10.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThreadPage : Page, IPageLifetimeCallback, IPageViewModelSource, IShellAppBarProvider, INavigationRolePage, INotifyPropertyChanged, INavigationDataPage, IWeakEventCallback
    {
        private object lifetimeToken;

        private readonly Stack<string> singleNavigationStack = new Stack<string>();

        public ThreadPage()
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
            if (channel?.Id == AppEvents.AppSuspendId)
            {
                OnSuspending(e as SuspendingEventArgs);
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
            navigatePostLink = null;
            if (link != null)
            {
                if ((link.LinkKind & BoardLinkKind.Post) != 0)
                {
                    navigatePostLink = link;
                }
            }
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
            savedTopPostHash = await GetStoredCurrentPostHash(navigatedLink);
            NavigatedTo?.Invoke(this, e);
        }

        private async Task<string> GetStoredCurrentPostHash(BoardLinkBase threadLink)
        {
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var link = await store.CurrentPostStore.GetCurrentPost(threadLink);
                if (link == null)
                {
                    return null;
                }
                return linkHash.GetLinkHash(link);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        private async Task SetStoredCurrentPost(BoardLinkBase threadLink, BoardLinkBase postLink, bool async = true)
        {
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                if (async)
                {
                    await store.CurrentPostStore.SetCurrentPost(threadLink, postLink);
                }
                else
                {
                    await store.CurrentPostStore.SetCurrentPostSync(threadLink, postLink);
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                var element = MainList.GetTopViewIndex();
                if (element?.Link != null)
                {
                    await SetStoredCurrentPost(navigatedLink, element.Link);
                }
            }
            NavigatedFrom?.Invoke(this, e);
        }

        private async void OnSuspending(SuspendingEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                var element = MainList.GetTopViewIndex();
                if (element?.Link != null)
                {
                    await SetStoredCurrentPost(navigatedLink, element.Link, false);
                }
            }
            catch
            {
                // ignore
            }
            finally
            {
                deferral.Complete();
            }
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
                if (restoredView == PageContentViews.SinglePostView && restoredSinglePostHash != null)
                {
                    var el = ViewModel.Posts.FirstOrDefault(t =>
                    {
                        var o = t?.Link;
                        if (o == null)
                        {
                            return false;
                        }
                        return linkHash.GetLinkHash(o) == restoredSinglePostHash;
                    });
                    if (el != null)
                    {
                        SingleSelectedItem = el;
                        SinglePostViewPopup.IsContentVisible = true;
                    }
                }
                restoredView = null;
                restoredSinglePostHash = null;
            });
        }

        private void ScrollIntoView(IPostViewModel post)
        {
            MainList.ScrollIntoView(post);
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
            var result = new Dictionary<string, object>();
            var element = SingleSelectedItem as IPostViewModel;
            var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
            if (element?.Link != null)
            {
                var hash = linkHash.GetLinkHash(element.Link);
                result["SingleListPost"] = hash;
            }
            result["CurrentView"] = (int)currentContentView;
            var stackArr = singleNavigationStack.ToArray();
            result["SingleNavigationStack"] = stackArr;
            return Task.FromResult(result);
        }

        private string restoredSinglePostHash;

        private PageContentViews? restoredView;

        /// <summary>
        /// Восстановить данные навигации.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат.</returns>
        public Task RestoreNavigationData(Dictionary<string, object> data)
        {
            if (data != null)
            {
                restoredSinglePostHash = null;
                if (data.ContainsKey("SingleListPost"))
                {
                    restoredSinglePostHash = (string) data["SingleListPost"];
                }
                restoredView = null;
                if (data.ContainsKey("CurrentView"))
                {
                    var v = (int)data["CurrentView"];
                    restoredView = (PageContentViews) v;
                }
                singleNavigationStack.Clear();
                if (data.ContainsKey("SingleNavigationStack"))
                {
                    var a = (string[]) data["SingleNavigationStack"];
                    if (a != null)
                    {
                        foreach (var l in a)
                        {
                            singleNavigationStack.Push(l);
                        }
                    }
                }
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

        private void MainList_OnShowFullPost(object sender, ShowFullPostEventArgs e)
        {
            SingleSelectedItem = e.Post;
            SinglePostViewPopup.IsContentVisible = true;
        }

        private enum PageContentViews
        {
            Default = 0,
            SinglePostView = 1
        }

        private PageContentViews currentContentView = PageContentViews.Default;

        private void ContentPopup_OnIsContentVisibleChanged(object sender, EventArgs e)
        {
            var v = sender as ContentPopup;
            if (v == null)
            {
                return;
            }
            if (v == SinglePostViewPopup && v.IsContentVisible)
            {
                currentContentView = PageContentViews.SinglePostView;
            }
            else
            {
                currentContentView = PageContentViews.Default;
            }

            if (v == SinglePostViewPopup && !v.IsContentVisible)
            {
                singleNavigationStack.Clear();
            }
        }

        /// <summary>
        /// Выбранный элемент.
        /// </summary>
        public object SingleSelectedItem
        {
            get { return (object) GetValue(SingleSelectedItemProperty); }
            set { SetValue(SingleSelectedItemProperty, value); }
        }

        /// <summary>
        /// Выбранный элемент.
        /// </summary>
        public static readonly DependencyProperty SingleSelectedItemProperty = DependencyProperty.Register("SingleSelectedItem", typeof (object), typeof (ThreadPage), new PropertyMetadata(null));
    }
}
