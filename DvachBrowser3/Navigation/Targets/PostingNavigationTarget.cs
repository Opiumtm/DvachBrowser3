using DvachBrowser3.Links;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Цель - постинг.
    /// </summary>
    public sealed class PostingNavigationTarget : PageNavigationTargetBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="postText">Текст поста.</param>
        public PostingNavigationTarget(BoardLinkBase link, IPostTextViewModel postText)
        {
            Link = link;
            PostText = postText;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        /// <summary>
        /// Текст поста.
        /// </summary>
        public IPostTextViewModel PostText { get; }

        /// <summary>
        /// Цитировать пост.
        /// </summary>
        public bool QuotePost { get; set; }
    }
}