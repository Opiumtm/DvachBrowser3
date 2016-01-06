using System;
using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Хост постинга.
    /// </summary>
    public sealed class PostingPointHost : IPostingPointHost
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parentLink">Родительская ссылка.</param>
        public PostingPointHost(BoardLinkBase parentLink)
        {
            if (parentLink == null) throw new ArgumentNullException(nameof(parentLink));
            ParentLink = parentLink;
        }

        /// <summary>
        /// Удачный постинг.
        /// </summary>
        /// <param name="newLink">Ссылка на новый тред или пост.</param>
        public void PostingSuccess(BoardLinkBase newLink)
        {
            OnSuccessfulPosting(new SuccessfulPostingEventArgs(ParentLink, newLink));
        }

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        public BoardLinkBase ParentLink { get; private set; }

        /// <summary>
        /// Успешный постинг.
        /// </summary>
        public event SuccessfulPostingEventHandler SuccessfulPosting;

        /// <summary>
        /// Информация о постинге.
        /// </summary>
        public IAsyncLazy<BoardReferencePostingExtension> PostingInfo { get; }

        /// <summary>
        /// Успешный постинг.
        /// </summary>
        /// <param name="e">Параметр.</param>
        private void OnSuccessfulPosting(SuccessfulPostingEventArgs e)
        {
            SuccessfulPostingEventHandler handler = SuccessfulPosting;
            handler?.Invoke(this, e);
        }
    }
}