using System.Threading.Tasks;
using DvachBrowser3.Board;
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
}