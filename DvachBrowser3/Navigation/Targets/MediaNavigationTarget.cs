using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Навигация на страницу просмотра медиа.
    /// </summary>
    public sealed class MediaNavigationTarget : PageNavigationTargetBase
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public MediaNavigationTarget(BoardLinkBase link)
        {
            Link = link;
        }

        /// <summary>
        /// Размер изображения в килобайтах.
        /// </summary>
        public double? SizeKb { get; set; }
    }
}