using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Удачный постинг.
    /// </summary>
    public sealed class PostingSuccessEventArgs : EventArgs
    {
        /// <summary>
        /// Редирект.
        /// </summary>
        public BoardLinkBase RedirectLink;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="redirectLink">Редирект.</param>
        public PostingSuccessEventArgs(BoardLinkBase redirectLink)
        {
            RedirectLink = redirectLink;
        }
    }

    /// <summary>
    /// Обработчик события по удачному постингу.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Событие.</param>
    public delegate void PostingSuccessEventHandler(object sender, PostingSuccessEventArgs e);
}