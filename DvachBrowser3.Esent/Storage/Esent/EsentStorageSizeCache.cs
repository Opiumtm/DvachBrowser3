using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Storage.Files;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Кэш размеров файлов.
    /// </summary>
    internal sealed class EsentStorageSizeCache : IStorageSizeCache
    {
        private readonly TableTransaction transaction;

        private readonly ISizeCacheAdapter adapter;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        /// <param name="adapter">Адаптер.</param>
        /// <param name="id">Идентификатор.</param>
        public EsentStorageSizeCache(TableTransaction transaction, ISizeCacheAdapter adapter, string id)
        {
            this.transaction = transaction;
            this.adapter = adapter;
            this.Id = id;
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            transaction.Dispose();
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Сохранить.
        /// </summary>
        public void Commit()
        {
            transaction.Transaction.Commit(CommitTransactionGrbit.None);
        }

        /// <summary>
        /// Установить размер файла.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <param name="size">Размер.</param>
        public void SetFileSize(string fileId, StorageSizeCacheItem size)
        {
            if (fileId == null) return;
            adapter.SetFileSize(transaction, new SizeCacheEsentItem()
            {
                FileId = fileId.ToLowerInvariant(),
                Size = size.Size,
                DTicks = size.Date.Ticks,
                OTicks = size.Date.Offset.Ticks
            });
        }

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        public ulong GetTotalSize()
        {
            return adapter.GetTotalSize(transaction);
        }

        /// <summary>
        /// Получить все элементы.
        /// </summary>
        /// <returns>Все элементы.</returns>
        public IEnumerable<KeyValuePair<string, StorageSizeCacheItem>> GetAllItems()
        {
            return
                adapter.GetAllItems(transaction)
                    .Select(
                        item => new KeyValuePair<string, StorageSizeCacheItem>(item.FileId, new StorageSizeCacheItem()
                        {
                            Size = item.Size,
                            Date = new DateTimeOffset(item.DTicks, new TimeSpan(item.OTicks))
                        }));
        }

        /// <summary>
        /// Получить все файлы.
        /// </summary>
        /// <returns>Список файлов.</returns>
        public IEnumerable<string> GetAllFiles()
        {
            return adapter.GetFileAllIds(transaction);
        }

        /// <summary>
        /// Удалить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        public void DeleteItem(string fileId)
        {
            if (fileId == null) return;
            adapter.DeleteItem(transaction, fileId.ToLowerInvariant());
        }

        /// <summary>
        /// Получить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns></returns>
        public StorageSizeCacheItem? GetItem(string fileId)
        {
            if (fileId == null)
            {
                return null;
            }
            var r = adapter.GetItem(transaction, fileId.ToLowerInvariant());
            if (r == null)
            {
                return null;
            }
            return new StorageSizeCacheItem()
            {
                Size = r.Value.Size,
                Date = new DateTimeOffset(r.Value.DTicks, new TimeSpan(r.Value.OTicks))
            };
        }

        /// <summary>
        /// Удалить все элементы.
        /// </summary>
        public void DeleteAllItems()
        {
            adapter.DeleteAllItems(transaction);
        }

        /// <summary>
        /// Проверка на существование.
        /// </summary>
        /// <param name="fileId">Идентификатор.</param>
        /// <returns>Результат.</returns>
        public bool IsItemPresent(string fileId)
        {
            if (fileId == null)
            {
                return false;
            }
            return adapter.IsItemPresent(transaction, fileId.ToLowerInvariant());
        }

        /// <summary>
        /// Получить количество элементов.
        /// </summary>
        /// <returns>Количество элементов.</returns>
        public int GetCount()
        {
            return adapter.GetCount(transaction);
        }
    }
}