using System;
using DvachBrowser3.Links;

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
        public BringIntoViewEventArgs(BoardLinkBase post)
        {
            Post = post;
        }

        /// <summary>
        /// Пост.
        /// </summary>
        public BoardLinkBase Post { get; private set; } 
    }
}