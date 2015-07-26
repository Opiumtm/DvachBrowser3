using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;
using Nito.AsyncEx;

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
            if (CollectionSource.PreloadedCollection != null)
            {
                SetData(Data);
            }
            CollectionSource.CollectionLoaded += CollectionSourceOnCollectionLoaded;
            if (collectionSource.AllowPosting)
            {
                PostingPoint = new PostingPointHost(collectionSource.Link);
                PostingPoint.SuccessfulPosting += PostingPointOnSuccessfulPosting;
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
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            throw new NotImplementedException();
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

        public IList<IPostCollectionFilterMode> Filters
        {
            get { throw new NotImplementedException(); }
        }

        public IPostCollectionFilterMode CurrentFilter
        {
            get { throw new NotImplementedException(); }
        }

        public void ResetFilter()
        {
            throw new NotImplementedException();
        }

        public ICommand ResetFilterCommand
        {
            get { throw new NotImplementedException(); }
        }

        public void RefreshFilter()
        {
            throw new NotImplementedException();
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
        }

        /// <summary>
        /// Сохранить состояние.
        /// </summary>
        /// <param name="pageState">Состояние страницы.</param>
        public override void OnSaveState(Dictionary<string, object> pageState)
        {
            base.OnSaveState(pageState);
            PostNavigation.OnSaveState(pageState);
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task AfterLoadState()
        {
            await base.AfterLoadState();
            await PostNavigation.AfterLoadState();
        }

        /// <summary>
        /// После сохранения состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task AfterSaveState()
        {
            await base.AfterSaveState();
            await PostNavigation.AfterSaveState();
        }

        /// <summary>
        /// Перед загрузкой состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task BeforeLoadState()
        {
            await base.BeforeLoadState();
            await PostNavigation.BeforeLoadState();
        }

        /// <summary>
        /// Перед сохранением состояния.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task BeforeSaveState()
        {
            await base.BeforeSaveState();
            await PostNavigation.BeforeSaveState();
        }

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task OnEnterPage()
        {
            await base.OnEnterPage();
            await PostNavigation.OnEnterPage();
            foreach (var p in Posts)
            {
                p.OnPageEntry();
            }
        }

        /// <summary>
        /// Выход со страницы.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task OnLeavePage()
        {
            await base.OnLeavePage();
            await PostNavigation.OnLeavePage();
        }
    }
}