using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Аргументы тапа на баннер.
    /// </summary>
    public sealed class BannerTappedEventArgs : EventArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.EventArgs"/>.
        /// </summary>
        public BannerTappedEventArgs(BoardLinkBase link)
        {
            Link = link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; private set; }
    }
}