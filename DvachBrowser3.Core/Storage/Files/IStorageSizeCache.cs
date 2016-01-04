using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Кэш размеров файлов.
    /// </summary>
    public interface IStorageSizeCache : IDisposable
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Сохранить.
        /// </summary>
        Task Commit();

        /// <summary>
        /// Установить размер файла.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <param name="size">Размер.</param>
        Task SetFileSize(string fileId, StorageSizeCacheItem size);

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        Task<ulong> GetTotalSize();

        /// <summary>
        /// Получить все элементы.
        /// </summary>
        /// <returns>Все элементы.</returns>
        Task<KeyValuePair<string, StorageSizeCacheItem>[]> GetAllItems();

        /// <summary>
        /// Получить все файлы.
        /// </summary>
        /// <returns>Список файлов.</returns>
        Task<string[]> GetAllFiles();

        /// <summary>
        /// Удалить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        Task DeleteItem(string fileId);

        /// <summary>
        /// Получить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns></returns>
        Task<StorageSizeCacheItem?> GetItem(string fileId);

        /// <summary>
        /// Удалить все элементы.
        /// </summary>
        Task DeleteAllItems();

        /// <summary>
        /// Проверка на существование.
        /// </summary>
        /// <param name="fileId">Идентификатор.</param>
        /// <returns>Результат.</returns>
        Task<bool> IsItemPresent(string fileId);

        /// <summary>
        /// Получить количество элементов.
        /// </summary>
        /// <returns>Количество элементов.</returns>
        Task<int> GetCount();
    }
}