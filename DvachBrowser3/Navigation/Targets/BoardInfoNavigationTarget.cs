using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Цель - информация о борде.
    /// </summary>
    public sealed class BoardInfoNavigationTarget : PageNavigationTargetBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BoardInfoNavigationTarget(BoardLinkBase link)
        {
            Link = link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }
    }
}