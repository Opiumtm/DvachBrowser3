using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовая модель коллекции постов.
    /// </summary>
    public abstract class PostCollectionViewModelBase : ViewModelBase, IPostCollectionViewModel
    {
        protected PostCollectionViewModelBase()
        {
            PostsByLink = new Dictionary<BoardLinkBase, IPostViewModel>(ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetComparer());
            var posts = new ObservableCollection<IPostViewModel>();
            Posts = posts;
            posts.CollectionChanged += PostsOnCollectionChanged;
        }

        private void PostsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    PostsByLink.Clear();
                    foreach (var p in Posts.ToArray().Where(p => p.Link != null))
                    {
                        PostsByLink[p.Link] = p;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (var p in e.NewItems.OfType<IPostViewModel>().ToArray().Where(p => p.Link != null))
                        {
                            PostsByLink[p.Link] = p;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var p in e.OldItems.OfType<IPostViewModel>().ToArray().Where(p => p.Link != null))
                        {
                            PostsByLink.Remove(p.Link);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {
                        foreach (var p in e.OldItems.OfType<IPostViewModel>().ToArray().Where(p => p.Link != null))
                        {
                            PostsByLink.Remove(p.Link);
                        }
                    }
                    if (e.NewItems != null)
                    {
                        foreach (var p in e.NewItems.OfType<IPostViewModel>().ToArray().Where(p => p.Link != null))
                        {
                            PostsByLink[p.Link] = p;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Посты.
        /// </summary>
        public IList<IPostViewModel> Posts { get; }

        /// <summary>
        /// Посты со ссылками.
        /// </summary>
        public IDictionary<BoardLinkBase, IPostViewModel> PostsByLink { get; }

        private IPostViewModel opPost;

        /// <summary>
        /// ОП-пост.
        /// </summary>
        public IPostViewModel OpPost
        {
            get { return opPost; }
            protected set
            {
                opPost = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        public event LinkClickEventHandler LinkClick;

        /// <summary>
        /// Может обновляться.
        /// </summary>
        public abstract bool CanUpdate { get; }

        /// <summary>
        /// Операция обновления данных.
        /// </summary>
        public abstract IOperationViewModel Update { get; }

        private bool hasData;

        /// <summary>
        /// Есть данные.
        /// </summary>
        public bool HasData
        {
            get { return hasData; }
            protected set
            {
                hasData = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Посты обновлены.
        /// </summary>
        public event EventHandler PostsUpdated;

        /// <summary>
        /// Создать модель представления поста.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Модель представления поста.</returns>
        protected abstract IPostViewModel CreatePostViewModel(PostTree post);

        /// <summary>
        /// Сливать и сортировать посты.
        /// </summary>
        protected virtual bool MergeAndSortPosts => true;

        /// <summary>
        /// Слить коллекцию с новыми постами.
        /// </summary>
        /// <param name="newPosts">Новая коллекция постов.</param>
        protected void MergePosts(IList<PostTree> newPosts)
        {
            if (newPosts == null)
            {
                Posts.Clear();
                return;
            }
            if (MergeAndSortPosts)
            {
                var equalityComparer = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetComparer();
                var comparer = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
                var updateHelper = new SortedCollectionUpdateHelper<IPostViewModel, PostTree, BoardLinkBase>(
                    equalityComparer,
                    comparer,
                    a => a.Link,
                    a => a.Link,
                    CreatePostViewModel,
                    (a, b) => a.PostStamp == b.PostStamp,
                    (a, b) => { },
                    newPosts,
                    Posts
                    );
                var update = updateHelper.GetUpdate();
                update.Added += UpdateOnAdded;
                update.Removed += UpdateOnRemoved;
                update.Update();
            }
            else
            {
                Posts.Clear();
                foreach (var p in newPosts)
                {
                    Posts.Add(CreatePostViewModel(p));
                }
            }
            OpPost = Posts.FirstOrDefault();
            UpdatePostCounters();
        }

        /// <summary>
        /// Слить коллекцию с новыми постами.
        /// </summary>
        /// <param name="newPosts">Новая коллекция постов.</param>
        protected void MergePosts(IList<IPostViewModel> newPosts)
        {
            if (newPosts == null)
            {
                Posts.Clear();
                return;
            }
            if (MergeAndSortPosts)
            {
                var equalityComparer = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetComparer();
                var comparer = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
                var updateHelper = new SortedCollectionUpdateHelper<IPostViewModel, IPostViewModel, BoardLinkBase>(
                    equalityComparer,
                    comparer,
                    a => a.Link,
                    a => a.Link,
                    a => a,
                    (a, b) => true,
                    (a, b) => { },
                    newPosts,
                    Posts
                    );
                var update = updateHelper.GetUpdate();
                update.Added += UpdateOnAdded;
                update.Removed += UpdateOnRemoved;
                update.Update();
            }
            else
            {
                Posts.Clear();
                foreach (var p in newPosts)
                {
                    Posts.Add(p);
                }
            }
            OpPost = Posts.FirstOrDefault();
            UpdatePostCounters();
        }

        /// <summary>
        /// Обновить счётчик постов.
        /// </summary>
        protected virtual void UpdatePostCounters()
        {
            for (int i = 0; i < Posts.Count; i++)
            {
                var c = Posts[i].Counter;
                var n = i + 1;
                if (c != n)
                {
                    Posts[i].Counter = n;
                }
            }
        }

        /// <summary>
        /// Привязывать события по клику.
        /// </summary>
        protected virtual bool AttachLinkClickEvents => true;

        private void UpdateOnRemoved(IPostViewModel postViewModel)
        {
            if (AttachLinkClickEvents)
            {
                postViewModel.Text.LinkClick -= TextOnLinkClick;
            }
        }

        private void UpdateOnAdded(IPostViewModel postViewModel)
        {
            if (AttachLinkClickEvents)
            {
                postViewModel.Text.LinkClick += TextOnLinkClick;
            }
        }

        private void TextOnLinkClick(object sender, LinkClickEventArgs e)
        {
            LinkClick?.Invoke(sender, e);
        }

        /// <summary>
        /// Посты обновлены.
        /// </summary>
        protected virtual void OnPostsUpdated()
        {
            PostsUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}