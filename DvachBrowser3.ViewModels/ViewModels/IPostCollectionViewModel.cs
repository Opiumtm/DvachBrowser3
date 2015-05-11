using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public interface IPostCollectionViewModel
    {
        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        void GotoPost(BoardLinkBase link);
    }
}