using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тайл посещённого треда.
    /// </summary>
    public sealed class VisitedThreadTileViewModel : ThreadTileViewModelBase<ShortThreadInfo>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public VisitedThreadTileViewModel(BoardLinkBase link) : base(link)
        {
        }
    }
}