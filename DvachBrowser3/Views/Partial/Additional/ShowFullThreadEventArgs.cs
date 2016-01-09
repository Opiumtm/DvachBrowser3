using System;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Параметры события показа треда целиком.
    /// </summary>
    public sealed class ShowFullThreadEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="postCollection">Коллекция постов.</param>
        /// <param name="post">Пост, с которого был совершён переход.</param>
        public ShowFullThreadEventArgs(IPostCollectionViewModel postCollection, IPostViewModel post)
        {
            PostCollection = postCollection;
            Post = post;
        }

        /// <summary>
        /// Коллекция постов.
        /// </summary>
        public IPostCollectionViewModel PostCollection { get; }

        /// <summary>
        /// Пост, с которого был совершён переход.
        /// </summary>
        public IPostViewModel Post { get; }
    }
}