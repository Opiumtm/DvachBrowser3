using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Цель - страница борды.
    /// </summary>
    public sealed class BoardPageNavigationTarget : PageNavigationTargetBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardPageNavigationTarget(BoardLinkBase link)
        {
            Link = link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }
    }
}