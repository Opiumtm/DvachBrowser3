using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    public interface IPostQuoteViewModel : IPostPartViewModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }
    }
}