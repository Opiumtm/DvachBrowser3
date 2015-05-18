using System;
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
        public PostCollectionLoadedEventArgs(PostTreeCollection collection)
        {
            Collection = collection;
        }

        /// <summary>
        /// Коллекция.
        /// </summary>
        public PostTreeCollection Collection { get; private set; } 
    }
}