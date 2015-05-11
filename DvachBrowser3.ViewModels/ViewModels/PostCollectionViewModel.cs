using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public sealed class PostCollectionViewModel : ViewModelBase, IPostCollectionViewModel
    {
        /// <summary>
        /// Перейти к посту.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public void GotoPost(BoardLinkBase link)
        {
            throw new System.NotImplementedException();
        }
    }
}