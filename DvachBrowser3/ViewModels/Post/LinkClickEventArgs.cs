using System;
using DvachBrowser3.TextRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргумент события клика на ссылку.
    /// </summary>
    public class LinkClickEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public LinkClickEventArgs(ITextRenderLinkAttribute link)
        {
            Link = link;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public ITextRenderLinkAttribute Link { get; }
    }
}