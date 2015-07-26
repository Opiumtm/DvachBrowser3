using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// �������� ��������� ��������.
    /// </summary>
    public class SuccessfulPostingEventArgs : EventArgs
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="parentLink">������������ ������.</param>
        /// <param name="newLink">����� ������ (���� ����).</param>
        public SuccessfulPostingEventArgs(BoardLinkBase parentLink, BoardLinkBase newLink)
        {
            ParentLink = parentLink;
            NewLink = newLink;
        }

        /// <summary>
        /// ������������ ������.
        /// </summary>
        public BoardLinkBase ParentLink { get; private set; }

        /// <summary>
        /// ����� ������ (���� ����).
        /// </summary>
        public BoardLinkBase NewLink { get; private set; }
    }
}