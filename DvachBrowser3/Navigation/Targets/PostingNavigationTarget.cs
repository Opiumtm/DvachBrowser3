using DvachBrowser3.Links;
using DvachBrowser3.ViewModels;

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
        /// <param name="postText">����� �����.</param>
        public PostingNavigationTarget(BoardLinkBase link, IPostTextViewModel postText)
        {
            Link = link;
            PostText = postText;
        }

        /// <summary>
        /// ������.
        /// </summary>
        public BoardLinkBase Link { get; }

        /// <summary>
        /// ����� �����.
        /// </summary>
        public IPostTextViewModel PostText { get; }

        /// <summary>
        /// ���������� ����.
        /// </summary>
        public bool QuotePost { get; set; }
    }
}