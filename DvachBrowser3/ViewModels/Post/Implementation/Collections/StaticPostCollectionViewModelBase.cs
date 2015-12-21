using System;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов без возможности обновления.
    /// </summary>
    /// <typeparam name="T">Тип коллекции.</typeparam>
    public abstract class StaticPostCollectionViewModelBase<T> : PostCollectionViewModelBase where T : IPostTreeListSource
    {
        /// <summary>
        /// Данные коллекции.
        /// </summary>
        protected T CollectionData { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        protected StaticPostCollectionViewModelBase(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            SetInitialData(data);
            CollectionData = data;
        }

        private async void SetInitialData(T data)
        {
            MergePosts(await data.GetPosts());
        }

        /// <summary>
        /// Может обновляться.
        /// </summary>
        public sealed override bool CanUpdate => false;

        /// <summary>
        /// Операция обновления данных.
        /// </summary>
        public sealed override IOperationViewModel Update => null;
    }
}