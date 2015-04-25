
using DvachBrowser3.Storage.Files;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения данных.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Директории данных.
        /// </summary>
        ICacheFolderInfo[] CacheFolders { get; }

        /// <summary>
        /// Маленькие изображения.
        /// </summary>
        IMediaStorage SmallImages { get; }

        /// <summary>
        /// Полноразмерные медиафайлы.
        /// </summary>
        IMediaStorage FullSizeMediaFiles { get; }
    }
}