using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Цель - тред.
    /// </summary>
    public sealed class ThreadNavigationTarget : PageNavigationTargetBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public ThreadNavigationTarget(BoardLinkBase link)
        {
            Link = link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }
    }
}