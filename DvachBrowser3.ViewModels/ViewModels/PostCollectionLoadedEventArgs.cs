using System;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргумент события по загрузке коллекции постов.
    /// </summary>
    public class PostCollectionLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="collection">Коллекция.</param>
        /// <param name="myPosts">Мои посты.</param>
        /// <param name="updateMode">Режим обновления.</param>
        public PostCollectionLoadedEventArgs(PostTreeCollection collection, MyPostsInfo myPosts, PostCollectionUpdateMode updateMode)
        {
            Collection = collection;
            MyPosts = myPosts;
            UpdateMode = updateMode;
        }

        /// <summary>
        /// Коллекция.
        /// </summary>
        public PostTreeCollection Collection { get; private set; }

        /// <summary>
        /// Режим обновления.
        /// </summary>
        public PostCollectionUpdateMode UpdateMode { get; private set; }

        /// <summary>
        /// Мои посты.
        /// </summary>
        public MyPostsInfo MyPosts { get; private set; }
    }
}