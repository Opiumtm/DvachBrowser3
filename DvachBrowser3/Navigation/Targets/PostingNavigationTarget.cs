using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// ���� - �������.
    /// </summary>
    public sealed class PostingNavigationTarget : PageNavigationTargetBase
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="link">������.</param>
        public PostingNavigationTarget(BoardLinkBase link)
        {
            Link = link;
        }

        /// <summary>
        /// ������.
        /// </summary>
        public BoardLinkBase Link { get; }
    }
}