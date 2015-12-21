using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// <summary>
        /// Посты.
        /// </summary>
        public IList<IPostViewModel> Posts { get; } = new ObservableCollection<IPostViewModel>();

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

        /// <summary>
        /// Создать модель представления поста.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Модель представления поста.</returns>
        protected abstract IPostViewModel CreatePostViewModel(PostTree post);

        /// <summary>
        /// Сливать и сортировать посты.
        /// </summary>
        protected virtual bool MergeAndSortPosts
        {
            get { return true; }
        }

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
                    Posts.Add(CreatePostViewModel(p));
                }
            }
            OpPost = Posts.FirstOrDefault();
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
    }
}