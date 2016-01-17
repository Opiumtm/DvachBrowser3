using System;
using Windows.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Параметр события получения изображения.
    /// </summary>
    public sealed class ImageSourceGotEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="cacheUri">URI кэша.</param>
        /// <param name="file">Файл.</param>
        public ImageSourceGotEventArgs(Uri cacheUri, StorageFile file)
        {
            CacheUri = cacheUri;
            File = file;
        }

        /// <summary>
        /// URI кэша.
        /// </summary>
        public Uri CacheUri { get; }

        /// <summary>
        /// Файл.
        /// </summary>
        public StorageFile File { get; }
    }
}