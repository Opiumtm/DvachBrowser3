using System;
using Windows.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргумент события по загрузке баннера.
    /// </summary>
    public class BannerLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="bannerFile">Файл баннера.</param>
        public BannerLoadedEventArgs(StorageFile bannerFile)
        {
            BannerFile = bannerFile;
        }

        /// <summary>
        /// Файл баннера.
        /// </summary>
        public StorageFile BannerFile { get; }
    }
}