using System;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргумент события отображения поста.
    /// </summary>
    public class BringIntoViewEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="post">Пост.</param>
        public BringIntoViewEventArgs(IPostViewModel post)
        {
            Post = post;
        }

        /// <summary>
        /// Пост.
        /// </summary>
        public IPostViewModel Post { get; private set; } 
    }
}