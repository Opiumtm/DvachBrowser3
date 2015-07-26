using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Место, из которого можно делать постинг.
    /// </summary>
    public interface IPostingPointHost
    {
        /// <summary>
        /// Удачный постинг.
        /// </summary>
        /// <param name="newLink">Ссылка на новый тред или пост.</param>
        void PostingSuccess(BoardLinkBase newLink);

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        BoardLinkBase ParentLink { get; }

        /// <summary>
        /// Успешный постинг.
        /// </summary>
        event SuccessfulPostingEventHandler SuccessfulPosting;
    }

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

    /// <summary>
    /// Обработчик события успешного постинга.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргумент.</param>
    public delegate void SuccessfulPostingEventHandler(object sender, SuccessfulPostingEventArgs e);
}