using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using DvachBrowser3.Configuration;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public sealed class PostCollectionViewModel : PageViewModelBase, IPostCollectionViewModel
    {
        private readonly string storagePrefix;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="collectionSource">Источник данных.</param>
        /// <param name="storagePrefix">Префикс для хранения.</param>
        public PostCollectionViewModel(IPostCollectionSource collectionSource, string storagePrefix = null)
        {
            if (collectionSource == null) throw new ArgumentNullException("collectionSource");
            CollectionSource = collectionSource;
            this.storagePrefix = storagePrefix ?? "";
            PostNavigation = new PostNavigation(this, CollectionSource.Link, this.storagePrefix);
            Posts = new ObservableCollection<IPostViewModel>();
            CollectionSource.CollectionLoaded += CollectionSourceOnCollectionLoaded;
            if (collectionSource.AllowPosting)
            {
                PostingPoint = new PostingPointHost(collectionSource.Link);
                PostingPoint.SuccessfulPosting += PostingPointOnSuccessfulPosting;
            }
            if (CollectionSource.PreloadedCollection != null)
            {
                SetData(Data);
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, InitializeData);
        }

        private async void InitializeData()
        {
            try
            {
                var loadOnStart = ServiceLocator.Current.GetServiceOrThrow<IViewConfigurationService>().NetworkProfile.LoadThreadOnStart;
                if (CollectionSource.SupportedUpdates != SupportedPostCollectionUpdates.None)
                {
                    var cached = await CollectionSource.LoadFromCache();
                    if ((loadOnStart == LoadThreadOnStartMode.Load || !cached) && CollectionSource.UpdateOperation != null)
                    {
                        CollectionSource.UpdateOperation.ExecuteOperation(PostCollectionUpdateMode.Default);
                        return;
                    }
                }
                else
                {
                    await CollectionSource.LoadFromCache();
                }
                if (loadOnStart == LoadThreadOnStartMode.CheckForUpdates && CollectionSource.CanCheckForUpdates)
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, CheckForUpdates);
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async void CheckForUpdates()
        {
            try
            {
                HasUpdates = await CollectionSource.CheckForUpdates() ?? HasUpdates;            
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // ignore
            }
        }

        private void CollectionSourceOnCollectionLoaded(object sender, PostCollectionLoadedEventArgs e)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SetData(e.Collection));
        }

        /// <summary>
        /// Тип коллекции.
        /// </summary>
        public PostCollectionKind Kind
        {
            get { return CollectionSource.Kind; }
        }

        /// <summary>
        /// Источник постов.
        /// </summary>
        public IPostCollectionSource CollectionSource { get; private set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public PostTreeCollection Data { get; private set; }

        /// <summary>
        /// Посты.
        /// </summary>
        public IList<IPostViewModel> Posts { get; private set; }

        private void SetData(PostTreeCollection data)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            Data = data;
            OnPropertyChanged("Data");
            OnPropertyChanged("PostsLoaded");
            SetPostModels();
            // ReSharper restore ExplicitCallerInfoArgument
        }

        private void SetPostModels()
        {
            if (Data != null)
            {
                var newPosts = (Data.Posts ?? new List<PostTree>()).Where(p => p.Link != null).ToList();
                var update = new SortedCollectionUpdateHelper<IPostViewModel, PostTree, BoardLinkBase>(
                    Services.GetServiceOrThrow<ILinkHashService>().GetComparer(),
                    Services.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer(),
                    s => s.Link,
                    s => s.Data.Link,
                    s => new PostViewModel(s, this),
                    CheckHashes,
                    newPosts,
                    Posts
                    );
                var diff = update.GetUpdate();
                diff.Update();
            }
            else
            {
                Posts.Clear();
            }
            Filtering.RefreshFilter();
        }

        private bool CheckHashes(PostTree src, IPostViewModel view)
        {
            if (src.Hash == null || view.Data.Hash == null)
            {
                return false;
            }
            return src.Hash == view.Hash;
        }

        /// <summary>
        /// Посты загружены.
        /// </summary>
        public bool PostsLoaded
        {
            get { return Data != null; }
        }

        /// <summary>
        /// Навигация по постам.
        /// </summary>
        public IPostNavigation PostNavigation { get; private set; }

        /// <summary>
        /// Найти пост.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Пост.</returns>
        public IPostViewModel FindPost(BoardLinkBase link)
        {
            var comparer = Services.GetServiceOrThrow<ILinkHashService>().GetComparer();
            return Posts.FirstOrDefault(p => comparer.Equals(link, p.Data.Link));
        }

        private void PostingPointOnSuccessfulPosting(object sender, SuccessfulPostingEventArgs successfulPostingEventArgs)
        {
        }

        /// <summary>
        /// Поинт постинга.
        /// </summary>
        public IPostingPointHost PostingPoint { get; private set; }

        /// <summary>
        /// Фильтрация.
        /// </summary>
        public IPostFiltering Filtering { get; private set; }

        private bool hasUpdates;

        /// <summary>
        /// Есть обновления.
        /// </summary>
        public bool HasUpdates
        {
            get { return hasUpdates; }
            private set
            {
                hasUpdates = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        public CancellationToken GetToken()
        {
            return CollectionSource.GetToken();
        }

        /// <summary>
        /// Загрузить состояние.
        /// </summary>
        /// <param name="navigationParameter">Параметр навигации.</param>
        /// <param name="pageState">Состояние страницы (null - нет состояния).</param>
        public override void OnLoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            base.OnLoadState(navigationParameter, pageState);
            PostNavigation.OnLoadState(navigationParameter, pageState);
            Filtering.OnLoadState(navigationParameter, pageState);
            if (pageState != null)
            {
                var hasUpdatesKey = storagePrefix + "_HasUpdates";
                if (pageState.ContainsKey(hasUpdatesKey))
                {
                    HasUpdates = (bool)pageState[hasUpdatesKey];
                }                
            }
            else
            {
                HasUpdates = false;
            }
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        public override void OnSaveState(Dictionary<string, object> pageState)
        {
            base.OnSaveState(pageState);
            PostNavigation.OnSaveState(pageState);
            Filtering.OnSaveState(pageState);
            var hasUpdatesKey = storagePrefix + "_HasUpdates";
            pageState[hasUpdatesKey] = HasUpdates;
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task AfterLoadState()
        {
            await base.AfterLoadState();
            await PostNavigation.AfterLoadState();
            await Filtering.AfterLoadState();
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task AfterSaveState()
        {
            await base.AfterSaveState();
            await PostNavigation.AfterSaveState();
            await Filtering.AfterSaveState();
        }

        /// <summary>
        /// Перед загрузкой состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task BeforeLoadState()
        {
            await base.BeforeLoadState();
            await PostNavigation.BeforeLoadState();
            await Filtering.BeforeLoadState();
        }

        /// <summary>
        /// Перед сохранением состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task BeforeSaveState()
        {
            await base.BeforeSaveState();
            await PostNavigation.BeforeSaveState();
            await Filtering.BeforeSaveState();
        }

        private DispatcherTimer updateCheckTimer;

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task OnEnterPage()
        {
            await base.OnEnterPage();
            await PostNavigation.OnEnterPage();
            await Filtering.OnEnterPage();
            foreach (var p in Posts)
            {
                p.OnPageEntry();
            }
            var networkProfile = ServiceLocator.Current.GetServiceOrThrow<IViewConfigurationService>().NetworkProfile;
            if (networkProfile.UpdateCheck && CollectionSource.CanCheckForUpdates)
            {
                updateCheckTimer = new DispatcherTimer();
                var interval = networkProfile.UpdateCheckPeriod;
                if (interval < TimeSpan.FromSeconds(5))
                {
                    interval = TimeSpan.FromSeconds(5);
                }
                updateCheckTimer.Interval = interval;
                updateCheckTimer.Tick += UpdateCheckTimerOnTick;
                updateCheckTimer.Start();
            }
        }

        private void UpdateCheckTimerOnTick(object sender, object o)
        {
            CheckForUpdates();
        }

        /// <summary>
        /// Выход со страницы.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task OnLeavePage()
        {
            await base.OnLeavePage();
            await PostNavigation.OnLeavePage();
            await Filtering.OnLeavePage();
            if (updateCheckTimer != null)
            {
                updateCheckTimer.Stop();
                updateCheckTimer = null;
            }
        }
    }
}