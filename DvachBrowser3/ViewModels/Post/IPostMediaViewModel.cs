using System.Collections.Generic;
using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиафайл поста.
    /// </summary>
    public interface IPostMediaViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Файлы.
        /// </summary>
        IList<IPostMediaFileViewModel> Files { get; }

        /// <summary>
        /// Основной файл.
        /// </summary>
        IPostMediaFileViewModel PrimaryFile { get; }

        /// <summary>
        /// Имеется медиа.
        /// </summary>
        bool HasMedia { get; }
    }
}