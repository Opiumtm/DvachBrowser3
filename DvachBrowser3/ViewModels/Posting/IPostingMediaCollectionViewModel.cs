using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Коллекция медиа для постинга.
    /// </summary>
    public interface IPostingMediaCollectionViewModel : IPostingFieldViewModel<Empty>
    {
        /// <summary>
        /// Изображения.
        /// </summary>
        IList<IPostingMediaViewModel> Media { get; }

        /// <summary>
        /// Можно добавлять медиа файл.
        /// </summary>
        bool CanAdd { get; }

        /// <summary>
        /// Добавить медиа файл.
        /// </summary>
        Task AddMediaFile(StorageFile file);

        /// <summary>
        /// Выбрать медиа файл.
        /// </summary>
        Task ChooseMediaFile();
    }
}