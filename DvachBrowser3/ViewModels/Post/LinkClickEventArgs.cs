using System;
using DvachBrowser3.TextRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// �������� ������� ����� �� ������.
    /// </summary>
    public class LinkClickEventArgs : EventArgs
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="link">������.</param>
        public LinkClickEventArgs(ITextRenderLinkAttribute link)
        {
            Link = link;
        }

        /// <summary>
        /// ������.
        /// </summary>
        public ITextRenderLinkAttribute Link { get; }
    }
}