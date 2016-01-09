using System;
using System.Linq;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Фильтрованная коллекция.
    /// </summary>
    public sealed class FilteredPostCollection : PostCollectionViewModelBase, IFilteredPostCollection
    {
        /// <summary>
        /// Может обновляться.
        /// </summary>
        public override bool CanUpdate => Source.CanUpdate;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="query">Запрос.</param>
        /// <param name="sortPosts">Сортировать посты.</param>
        public FilteredPostCollection(IPostCollectionViewModel source, IPostCollectionSearchQuery query, bool sortPosts = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (query == null) throw new ArgumentNullException(nameof(query));
            Source = source;
            Query = query;
            MergeAndSortPosts = sortPosts;
            Source.PostsUpdated += SourceOnPostsUpdated;
            SetPosts();
        }

        private void SourceOnPostsUpdated(object sender, EventArgs eventArgs)
        {
            SetPosts();
            OnPostsUpdated();
        }

        private void SetPosts()
        {
            if (Source.Posts == null)
            {
                Posts.Clear();
            }
            else
            {
                MergePosts(Source.Posts.Where(Query.Filter).ToList());
            }
        }

        /// <summary>
        /// Операция обновления данных.
        /// </summary>
        public override IOperationViewModel Update => Source.Update;

        /// <summary>
        /// Создать модель представления поста.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Модель представления поста.</returns>
        protected override IPostViewModel CreatePostViewModel(PostTree post)
        {
            // Для этого тип коллекции не поддерживается прямое обновление.
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Исходная коллекция.
        /// </summary>
        public IPostCollectionViewModel Source { get; }

        /// <summary>
        /// Запрос.
        /// </summary>
        public IPostCollectionSearchQuery Query { get; }

        /// <summary>
        /// Сливать и сортировать посты.
        /// </summary>
        protected override bool MergeAndSortPosts { get; }
    }
}