
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

        /// <summary>
        /// Данные тредов.
        /// </summary>
        IThreadDataStorage ThreadData { get; }

        /// <summary>
        /// Данные постов.
        /// </summary>
        IPostDataStorage PostData { get; }

        /// <summary>
        /// Черновики.
        /// </summary>
        IDraftDataStorage Drafts { get; }

        /// <summary>
        /// Архивы.
        /// </summary>
        IArchiveStore Archives { get; }

        /// <summary>
        /// Хранилище текущих постов.
        /// </summary>
        ICurrentPostStore CurrentPostStore { get; }

        /// <summary>
        /// Любые данные.
        /// </summary>
        ICustomDataStore CustomData { get; }
    }
}