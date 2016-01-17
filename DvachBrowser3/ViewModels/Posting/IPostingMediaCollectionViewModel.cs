using System.Collections.Generic;
using System.ComponentModel;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция медиа для постинга.
    /// </summary>
    public interface IPostingMediaCollectionViewModel : IPostingFieldViewModel<PostingMediaFiles>
    {
        /// <summary>
        /// Изображения.
        /// </summary>
        IList<IPostingMediaViewModel> Media { get; }
    }
}