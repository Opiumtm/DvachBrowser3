using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргумент успешного постинга.
    /// </summary>
    public class SuccessfulPostingEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parentLink">Родительская ссылка.</param>
        /// <param name="newLink">Новая ссылка (если есть).</param>
        public SuccessfulPostingEventArgs(BoardLinkBase parentLink, BoardLinkBase newLink)
        {
            ParentLink = parentLink;
            NewLink = newLink;
        }

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        public BoardLinkBase ParentLink { get; private set; }

        /// <summary>
        /// Новая ссылка (если есть).
        /// </summary>
        public BoardLinkBase NewLink { get; private set; }
    }
}