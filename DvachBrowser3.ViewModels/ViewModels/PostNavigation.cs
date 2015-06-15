using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Навигация по постам.
    /// </summary>
    public sealed class PostNavigation : PageViewModelBase, IPostNavigation
    {
        private readonly BoardLinkBase threadLink;

        private readonly BoardLinkBase topPostLink;

        private BoardLinkBase topViewPost;

        private readonly string storagePrefix;

        private Stack<BoardLinkBase> navigationStack = new Stack<BoardLinkBase>();

        private readonly IComparer<BoardLinkBase> postComparer;

        private readonly IPostCollectionViewModel parent;

        public PostNavigation(IPostCollectionViewModel parent, BoardLinkBase threadLink, string storagePrefix = null)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (threadLink == null) throw new ArgumentNullException("threadLink");
            if (threadLink.LinkKind != BoardLinkKind.Thread)
            {
                throw new ArgumentException("Неправильный тип ссылки на тред.");
            }
            this.threadLink = threadLink;
            this.parent = parent;
            this.topPostLink = Services.GetServiceOrThrow<ILinkTransformService>().GetRootPostLink(this.threadLink);
            TopViewPost = topPostLink;
            this.storagePrefix = storagePrefix ?? "";
            NavigationChanged();
            ClearNavigationCommand = new ViewModelRelayCommand(ClearNavigationStack);
            backCommand = new ViewModelRelayCommand(Back, () => CanGoBack);
            postComparer = Services.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
        }

        private bool topPostLoaded = false;

        /// <summary>
        /// Загрузить состояние.
        /// </summary>
        /// <param name="navigationParameter">Параметр навигации.</param>
        /// <param name="pageState">Состояние страницы (null - нет состояния).</param>
        public override void OnLoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            if (pageState != null)
            {
                var key = storagePrefix + "_PostNavigation_Stack";
                var value = pageState.ContainsKey(key) ? (IEnumerable<BoardLinkBase>)(pageState[key]) : null;
                navigationStack = new Stack<BoardLinkBase>(value);
                NavigationChanged();
                var key2 = storagePrefix + "_PostNavigation_TopView";
                var value2 = pageState.ContainsKey(key2) ? (BoardLinkBase)pageState[key2] : null;
                topPostLoaded = value2 != null;
                TopViewPost = value2 ?? topPostLink;                
            }
            else
            {
                TopViewPost = topPostLink;
            }
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task AfterLoadState()
        {
            await base.AfterLoadState();
            try
            {
                var store = Services.GetServiceOrThrow<IStorageService>();
                var post = !topPostLoaded ? await store.CurrentPostStore.GetCurrentPost(threadLink) ?? TopViewPost : TopViewPost;
                if (post != null)
                {
                    BringIntoView(post);
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        public override void OnSaveState(Dictionary<string, object> pageState)
        {
            var key = storagePrefix + "_PostNavigation_Stack";
            var value = navigationStack.ToArray();
            pageState[key] = value;
            if (TopViewPost != null)
            {
                var key2 = storagePrefix + "_PostNavigation_TopView";
                pageState[key2] = TopViewPost;
            }
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task AfterSaveState()
        {
            await base.AfterSaveState();
            var store = Services.GetServiceOrThrow<IStorageService>();
            await store.CurrentPostStore.SetCurrentPostSync(threadLink, TopViewPost);
        }

        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public void GotoPost(BoardLinkBase link)
        {
            try
            {
                if (link.LinkKind == BoardLinkKind.Post)
                {
                    var thisThread = Services.GetServiceOrThrow<ILinkTransformService>().IsThisTread(threadLink, link);
                    if (thisThread)
                    {
                        PushTopPost();
                        BringIntoView(link);
                    }
                    else
                    {
                        var key = LinkKeyService.GetKey(link);
                        Services.GetServiceOrThrow<INavigationService>().Navigate(ViewModelConstants.Pages.Thread, key);
                    }                    
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private void PushTopPost()
        {
            bool ignore = false;
            if (navigationStack.Count > 0 && TopViewPost != null)
            {
                var comparer = Services.GetServiceOrThrow<ILinkHashService>().GetComparer();
                var top = navigationStack.Peek();
                if (comparer.Equals(top, TopViewPost))
                {
                    ignore = true;
                }
            }
            if (TopViewPost != null && !ignore)
            {
                navigationStack.Push(TopViewPost);
            }
        }

        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="number">Номер.</param>
        public void GotoPost(int number)
        {
            try
            {
                var post = Services.GetServiceOrThrow<ILinkTransformService>().GetPostLinkByNum(threadLink, number);
                var comparer = Services.GetServiceOrThrow<ILinkHashService>().GetComparer();
                var hasPost = parent.Posts.Any(t => comparer.Equals(post, t.Data.Link));
                if (!hasPost)
                {
                    if (number <= parent.Posts.Count && number >= 1)
                    {
                        post = parent.Posts[number - 1].Data.Link;
                    }
                }
                GotoPost(post);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Пост попал в отображение.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        public void PostGotIntoView(BoardLinkBase id)
        {
            if (id != null)
            {
                if (Services.GetServiceOrThrow<ILinkTransformService>().IsThisTread(threadLink, id))
                {
                    if (TopViewPost == null)
                    {
                        TopViewPost = id;
                    }
                    else
                    {
                        var tvp = TopViewPost;
                        if (postComparer.Compare(id, tvp) < 0)
                        {
                            TopViewPost = id;
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// Отобразить пост.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        public void BringIntoView(BoardLinkBase id)
        {
            if (id != null)
            {
                if (Dispatcher != null)
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        try
                        {
                            OnNeedBringIntoView(new BringIntoViewEventArgs(id));
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.BreakOnError(ex);
                        }
                    });
                }
                else
                {
                    try
                    {
                        OnNeedBringIntoView(new BringIntoViewEventArgs(id));                
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Нужно отобразить пост.
        /// </summary>
        public event BringIntoViewEventHandler NeedBringIntoView;

        private void OnNeedBringIntoView(BringIntoViewEventArgs e)
        {
            BringIntoViewEventHandler handler = NeedBringIntoView;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Верхний отображаемый пост.
        /// </summary>
        public BoardLinkBase TopViewPost
        {
            get { return topViewPost; }
            set
            {
                topViewPost = value;
                OnPropertyChanged();
            }
        }

        public bool CanGoBack
        {
            get { return navigationStack.Count > 0; }
        }

        public void Back()
        {
            if (CanGoBack)
            {
                var post = navigationStack.Pop();
                BringIntoView(post);
            }
        }

        private readonly ViewModelRelayCommand backCommand;

        /// <summary>
        /// Команда назад.
        /// </summary>
        public ICommand BackCommand
        {
            get { return backCommand; }
        }

        /// <summary>
        /// Команда сброса навигации.
        /// </summary>
        public ICommand ClearNavigationCommand { get; private set; }

        public void ClearNavigationStack()
        {
            navigationStack.Clear();
            NavigationChanged();
        }

        public BoardLinkBase TopNavigation
        {
            get
            {
                var post = navigationStack.Count > 0 ? navigationStack.Peek() : null;
                return post ?? topPostLink;
            }
        }

        private void NavigationChanged()
        {
            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("TopNavigation");
            OnPropertyChanged("CanGoBack");
            backCommand.RaiseCanExecuteChanged();
            // ReSharper restore ExplicitCallerInfoArgument
        }
    }
}