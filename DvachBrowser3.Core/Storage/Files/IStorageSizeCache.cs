using System;
using System.Collections.Generic;

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
        void Commit();

        /// <summary>
        /// Установить размер файла.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <param name="size">Размер.</param>
        void SetFileSize(string fileId, StorageSizeCacheItem size);

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        ulong GetTotalSize();

        /// <summary>
        /// Получить все элементы.
        /// </summary>
        /// <returns>Все элементы.</returns>
        IEnumerable<KeyValuePair<string, StorageSizeCacheItem>> GetAllItems();

        /// <summary>
        /// Получить все файлы.
        /// </summary>
        /// <returns>Список файлов.</returns>
        IEnumerable<string> GetAllFiles();

        /// <summary>
        /// Удалить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        void DeleteItem(string fileId);

        /// <summary>
        /// Получить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns></returns>
        StorageSizeCacheItem? GetItem(string fileId);

        /// <summary>
        /// Удалить все элементы.
        /// </summary>
        void DeleteAllItems();

        /// <summary>
        /// Проверка на существование.
        /// </summary>
        /// <param name="fileId">Идентификатор.</param>
        /// <returns>Результат.</returns>
        bool IsItemPresent(string fileId);

        /// <summary>
        /// Получить количество элементов.
        /// </summary>
        /// <returns>Количество элементов.</returns>
        int GetCount();
    }
}