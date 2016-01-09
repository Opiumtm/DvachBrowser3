using System;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Аргуенты события показа полного поста.
    /// </summary>
    public sealed class ShowFullPostEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="post">Пост.</param>
        public ShowFullPostEventArgs(IPostViewModel post)
        {
            Post = post;
        }

        /// <summary>
        /// Пост.
        /// </summary>
        public IPostViewModel Post { get; }
    }
}