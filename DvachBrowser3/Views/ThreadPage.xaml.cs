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
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Storage;
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
    public sealed partial class ThreadPage : Page, IPageLifetimeCallback, IPageViewModelSource, IDynamicShellAppBarProvider, INavigationRolePage, INotifyPropertyChanged, INavigationDataPage, IWeakEventCallback, INavigationLinkCallback
    {
        private object lifetimeToken;

        private readonly Stack<string> singleNavigationStack = new Stack<string>();

        public ThreadPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            ViewModelOnPropertyChanged = CreateViewModelOnPropertyChangedHandler(new WeakReference<ThreadPage>(this));
            OnPostsUpdated = CreateOnPostsUpdatedHandler(new WeakReference<ThreadPage>(this));
            this.DataContext = this;
            lifetimeToken = this.BindAppLifetimeEvents();
            this.Unloaded += (sender, e) =>
            {
                ViewModel = null;
            };
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            isLoaded = true;
            isLoaded2 = true;
            RestoreTopPost();
            RestoreDataView();
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
                if ((link.LinkKind & BoardLinkKind.Post) != 0 && e.NavigationMode != NavigationMode.Back)
                {
                    navigatePostLink = link;
                }
            }
            if (navigatedLink == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            savedTopPostHash = await GetStoredCurrentPostHash(navigatedLink);
            isTopPostSet = true;
            var vm = new ThreadViewModel(navigatedLink);
            ViewModel = vm;
            vm.PostsUpdated += OnPostsUpdated;
            isBackNavigated = e.NavigationMode == NavigationMode.Back;
            vm.IsBackNavigatedToViewModel = isBackNavigated;
            vm.PropertyChanged += ViewModelOnPropertyChanged;
            ViewModelOnPropertyChanged(vm, new PropertyChangedEventArgs(null));
            NavigatedTo?.Invoke(this, e);
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

        // ReSharper disable once InconsistentNaming
        private readonly PropertyChangedEventHandler ViewModelOnPropertyChanged;

        private static PropertyChangedEventHandler CreateViewModelOnPropertyChangedHandler(WeakReference<ThreadPage> handle)
        {
            return (sender, e) =>
            {
                ThreadPage obj;
                if (handle.TryGetTarget(out obj))
                {
                    obj.ViewModelOnPropertyChangedHandler(sender, e);
                }
            };
        }

        private void ViewModelOnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var vm = sender as IThreadViewModel;
            if (vm != null && object.ReferenceEquals(vm, ViewModel))
            {
                if (e.PropertyName == nameof(IThreadViewModel.IsUpdated) || e.PropertyName == null)
                {
                    var isUpdated = vm.IsUpdated;
                    if (NewPostsIndicator.Visibility == Visibility.Collapsed && isUpdated)
                    {
                        (Resources["ShowNewPostsAnimation"] as Storyboard)?.Begin();
                    }
                    if (NewPostsIndicator.Visibility == Visibility.Visible && !isUpdated)
                    {
                        (Resources["HideNewPostsAnimation"] as Storyboard)?.Begin();
                    }
                }
            }
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

        // ReSharper disable once InconsistentNaming
        private readonly EventHandler OnPostsUpdated;

        private static EventHandler CreateOnPostsUpdatedHandler(WeakReference<ThreadPage> handle)
        {
            return (sender, e) =>
            {
                ThreadPage obj;
                if (handle.TryGetTarget(out obj))
                {
                    obj.OnPostsUpdatedHandler(sender, e);
                }
            };
        }


        private void OnPostsUpdatedHandler(object sender, EventArgs eventArgs)
        {
            RestoreTopPost();
            RestoreDataView();
        }

        private bool isLoaded = false;
        private bool isLoaded2 = false;

        private bool isTopPostSet = false;

        private void RestoreTopPost()
        {
            var vm = ViewModel;
            if (vm == null)
            {
                return;
            }
            if (!isLoaded || !isTopPostSet)
            {
                return;
            }
            AppHelpers.DispatchAction(() =>
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
                    var el = vm.Posts.FirstOrDefault(t =>
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
                return Task.CompletedTask;
            }, false, 50);
        }

        private bool dataViewLoaded = false;

        private void RestoreDataView()
        {
            var vm = ViewModel;
            if (vm == null)
            {
                return;
            }
            if (!vm.HasData)
            {
                return;
            }
            if (!dataViewLoaded)
            {
                return;
            }
            if (!isLoaded2)
            {
                return;
            }

            AppHelpers.DispatchAction(() =>
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                if (restoredView == PageContentViews.SinglePostView && restoredSinglePostHash != null)
                {
                    var el = vm.Posts.FirstOrDefault(t =>
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
                        AppHelpers.DispatchAction(() =>
                        {
                            SinglePostViewPopup.IsContentVisible = true;
                            return Task.CompletedTask;
                        });
                    }
                }
                restoredView = null;
                restoredSinglePostHash = null;
                return Task.CompletedTask;
            }, false, 60);
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

            var postButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Edit),
                Label = "Написать"
            };
            postButton.Click += async (sender, e) =>
            {
                try
                {
                    var opPost = ViewModel?.OpPost;
                    if (opPost != null)
                    {
                        ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new PostingNavigationTarget(opPost.ParentLink, opPost.Text) );
                    }
                }
                catch (Exception ex)
                {
                    await AppHelpers.ShowError(ex);
                }
            };

            var syncButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Sync),
                Label = "Обновить"
            };
            syncButton.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Update.CanStart") });
            syncButton.Click += (sender, e) => ViewModel?.Synchronize();

            appBar.PrimaryCommands.Add(postButton);

            if (currentContentView == PageContentViews.Default)
            {
                short downNum, upNum;
                unchecked
                {
                    downNum = (short)0xE74B;
                    upNum = (short)0xE74A;
                }

                var downButton = new AppBarButton()
                {
                    Icon = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = new string(new[] { (char)downNum }) },
                    Label = "Вниз"
                };
                downButton.Click += (sender, e) => Down();

                var upButton = new AppBarButton()
                {
                    Icon = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = new string(new[] { (char)upNum }) },
                    Label = "Вверх"
                };
                upButton.Click += (sender, e) => Up();

                appBar.PrimaryCommands.Add(upButton);
                appBar.PrimaryCommands.Add(downButton);
            } else if (currentContentView == PageContentViews.SinglePostView)
            {
                var backButton = new AppBarButton()
                {
                    Icon = new SymbolIcon(Symbol.Back),
                    Label = "Назад"
                };
                backButton.Click += (sender, e) => SingleListBack();

                var goButton = new AppBarButton()
                {
                    Icon = new SymbolIcon(Symbol.Go),
                    Label = "Перейти"
                };
                goButton.Click += (sender, e) => SingleListGo();
                appBar.PrimaryCommands.Add(backButton);
                appBar.PrimaryCommands.Add(goButton);
            }


            appBar.PrimaryCommands.Add(syncButton);

            var loadAllButton = new AppBarButton()
            {
                Label = "Загрузить заново",
            };
            loadAllButton.Click += (sender, e) =>
            {
                ViewModel?.FullReload();
            };

            var addToFavorites = new AppBarButton()
            {
                Label = "В избранное",
            };
            addToFavorites.Click += async (sender, e) =>
            {
                try
                {
                    var vm = ViewModel;
                    if (vm != null)
                    {
                        await vm.AddToFavorites();
                    }
                }
                catch (Exception ex)
                {
                    await AppHelpers.ShowError(ex);
                }
            };

            appBar.SecondaryCommands.Add(loadAllButton);
            appBar.SecondaryCommands.Add(addToFavorites);

            return appBar;
        }

        private void Down()
        {
            var vm = ViewModel?.Posts?.LastOrDefault();
            if (vm != null)
            {
                MainList.ScrollIntoView(vm);
                SinglePostViewPopup.IsContentVisible = false;
            }
        }

        private void Up()
        {
            var vm = ViewModel?.Posts?.FirstOrDefault();
            if (vm != null)
            {
                MainList.ScrollIntoView(vm);
                SinglePostViewPopup.IsContentVisible = false;
            }
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
            dataViewLoaded = true;
            RestoreDataView();
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
        public IThreadViewModel ViewModel
        {
            get { return (IThreadViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IThreadViewModel), typeof (ThreadPage), new PropertyMetadata(null, ViewModelPropertyChangedCallback));

        private static void ViewModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ThreadPage;
            if (obj != null)
            {
                obj.PostCollection = e.NewValue as IPostCollectionViewModel;
            }
        }

        /// <summary>
        /// Коллекция постов.
        /// </summary>
        public IPostCollectionViewModel PostCollection
        {
            get { return (IPostCollectionViewModel) GetValue(PostCollectionProperty); }
            set { SetValue(PostCollectionProperty, value); }
        }

        /// <summary>
        /// Коллекция постов.
        /// </summary>
        public static readonly DependencyProperty PostCollectionProperty = DependencyProperty.Register("PostCollection", typeof (IPostCollectionViewModel), typeof (ThreadPage), new PropertyMetadata(null));

        private async void MainList_OnShowFullPost(object sender, ShowFullPostEventArgs e)
        {
            try
            {
                singleNavigationStack.Clear();
                SingleSelectedItem = e.Post;
                await Task.Yield();
                SinglePostViewPopup.IsContentVisible = true;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
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
            var ocv = currentContentView;
            if (v == SinglePostViewPopup && v.IsContentVisible)
            {
                currentContentView = PageContentViews.SinglePostView;
            }
            else
            {
                currentContentView = PageContentViews.Default;
            }

            if (currentContentView != ocv)
            {
                AppBarChange?.Invoke(this, EventArgs.Empty);
            }

            if (v == SinglePostViewPopup && !v.IsContentVisible)
            {
                singleNavigationStack.Clear();
            }
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (SinglePostViewPopup.IsContentVisible)
                {
                    SinglePostViewPopup.IsContentVisible = false;
                    e.Cancel = true;
                }
            }
            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Выбранный элемент.
        /// </summary>
        public object SingleSelectedItem
        {
            get
            {
                return SingleList?.SelectedItem;
            }
            set
            {
                if (SingleList != null)
                {
                    SingleList.SelectedItem = value;
                }
            }
        }

        private void PushCurrentLink(BoardLinkBase l)
        {
            var si = SingleSelectedItem as IPostViewModel;
            PushCurrentLink(si, l);
        }

        private void PushCurrentLink(IPostViewModel si, BoardLinkBase l)
        {
            if (si?.Link != null)
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var oldHash = linkHash.GetLinkHash(si.Link);
                if (ViewModel?.FindPost(new LinkHashPostCollectionSearchQuery(oldHash)) == null)
                {
                    return;
                }
                if (l != null)
                {
                    var newHash = linkHash.GetLinkHash(l);
                    if (newHash == oldHash)
                    {
                        return;
                    }
                }
                singleNavigationStack.Push(oldHash);
            }
        }

        /// <summary>
        /// Обработать событие по переходу по ссылке.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Событие.</param>
        /// <returns>true, если обработка произведена и действий по умолчанию не требуется.</returns>
        public bool HandleNavigationLinkClick(object sender, LinkClickEventArgs e)
        {
            var vm = ViewModel;
            if (vm == null)
            {
                return false;
            }
            var l0 = e?.Link?.CustomData as BoardLinkBase;
            if (l0 != null)
            {
                var l = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetPostLinkFromAnyLink(l0);
                if (l != null)
                {
                    if (vm.PostsByLink.ContainsKey(l))
                    {
                        var p = vm.PostsByLink[l];
                        if (p != null)
                        {
                            if (!SinglePostViewPopup.IsContentVisible)
                            {
                                singleNavigationStack.Clear();
                            }
                            var lvm = e.LinkContext as IPostViewModel;
                            var lc = lvm?.Link;
                            if (lc != null)
                            {
                                if (SinglePostViewPopup.IsContentVisible)
                                {
                                    PushCurrentLink(lc);
                                }
                                PushCurrentLink(lvm, l);
                            }
                            else
                            {
                                PushCurrentLink(l);
                            }
                            SingleSelectedItem = p;
                            AppHelpers.DispatchAction(() =>
                            {
                                SinglePostViewPopup.IsContentVisible = true;
                                return Task.CompletedTask;
                            });
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void ThreadPage_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                e.Handled = true;
                SinglePostViewPopup.IsContentVisible = false;
            }
        }

        private void SingleListBack()
        {
            var vm = ViewModel;
            if (vm == null)
            {
                return;
            }
            if (SinglePostViewPopup.IsContentVisible)
            {
                if (singleNavigationStack.Count > 0)
                {
                    var hash = singleNavigationStack.Pop();
                    var p = vm.FindPost(new LinkHashPostCollectionSearchQuery(hash));
                    if (p != null)
                    {
                        SingleSelectedItem = p;
                    }
                }
                else
                {
                    SinglePostViewPopup.IsContentVisible = false;
                }
            }
        }

        private void SingleList_OnBackButtonClick(object sender, EventArgs e)
        {
            SingleListBack();
        }

        private void SingleListGo()
        {
            var si = SingleSelectedItem as IPostViewModel;
            if (SinglePostViewPopup.IsContentVisible && si != null)
            {
                MainList.ScrollIntoView(si);
                SinglePostViewPopup.IsContentVisible = false;
            }
        }

        private void SingleList_OnGoButtonClick(object sender, EventArgs e)
        {
            SingleListGo();
        }

        /// <summary>
        /// Изменить строку команд.
        /// </summary>
        public event EventHandler AppBarChange;

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();

        private void NewPostsIndicator_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel?.CleanUpdated();
        }

        private void SingleList_OnSelectedItemChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SingleSelectedItem));
        }
    }
}
