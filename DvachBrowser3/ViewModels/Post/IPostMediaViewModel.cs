using System.Collections.Generic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиафайл поста.
    /// </summary>
    public interface IPostMediaViewModel : IPostPartViewModel
    {
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