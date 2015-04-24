using System;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Элемент кэша размеров хранилища.
    /// </summary>
    public struct StorageSizeCacheItem
    {
        /// <summary>
        /// Размер.
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTimeOffset Date { get; set; }
    }
}