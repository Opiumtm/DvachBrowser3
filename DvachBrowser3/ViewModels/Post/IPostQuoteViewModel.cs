using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    public interface IPostQuoteViewModel : IPostPartViewModel
    {
        /// <summary>
        /// ���.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ������.
        /// </summary>
        BoardLinkBase Link { get; }
    }
}