using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task Commit()
        {
            await transaction.ExecutionContext.Execute(() =>
            {
                transaction.Transaction.Commit(CommitTransactionGrbit.None);
            });
        }

        /// <summary>
        /// Установить размер файла.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <param name="size">Размер.</param>
        public async Task SetFileSize(string fileId, StorageSizeCacheItem size)
        {
            await transaction.ExecutionContext.Execute(() =>
            {
                if (fileId == null) return;
                adapter.SetFileSize(transaction, new SizeCacheEsentItem()
                {
                    FileId = fileId.ToLowerInvariant(),
                    Size = size.Size,
                    DTicks = size.Date.Ticks,
                    OTicks = size.Date.Offset.Ticks
                });
            });
        }

        /// <summary>
        /// Установить размер файла.
        /// </summary>
        /// <param name="sizes">Размеры.</param>
        public async Task SetFilesSize(KeyValuePair<string, StorageSizeCacheItem>[] sizes)
        {
            if (sizes == null)
            {
                return;
            }
            await transaction.ExecutionContext.Execute(() =>
            {
                foreach (var kv in sizes)
                {
                    var fileId = kv.Key;
                    var size = kv.Value;
                    if (fileId == null) continue;
                    adapter.SetFileSize(transaction, new SizeCacheEsentItem()
                    {
                        FileId = fileId.ToLowerInvariant(),
                        Size = size.Size,
                        DTicks = size.Date.Ticks,
                        OTicks = size.Date.Offset.Ticks
                    });
                }
            });
        }

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        public async Task<ulong> GetTotalSize()
        {
            return await transaction.ExecutionContext.Execute(() => adapter.GetTotalSize(transaction));
        }

        /// <summary>
        /// Получить общий размер и количество.
        /// </summary>
        /// <returns>Общий размер и количество.</returns>
        public async Task<Tuple<ulong, int>> GetTotalSizeAndCount()
        {
            return await transaction.ExecutionContext.Execute(() => adapter.GetTotalSizeAndCount(transaction));
        }

        /// <summary>
        /// Получить все элементы.
        /// </summary>
        /// <returns>Все элементы.</returns>
        public async Task<KeyValuePair<string, StorageSizeCacheItem>[]> GetAllItems()
        {
            return await transaction.ExecutionContext.Execute(() =>
            {
                return
                    adapter.GetAllItems(transaction)
                        .Select(
                            item => new KeyValuePair<string, StorageSizeCacheItem>(item.FileId, new StorageSizeCacheItem()
                            {
                                Size = item.Size,
                                Date = new DateTimeOffset(item.DTicks, new TimeSpan(item.OTicks))
                            })).ToArray();
            });
        }

        /// <summary>
        /// Получить все файлы.
        /// </summary>
        /// <returns>Список файлов.</returns>
        public async Task<string[]> GetAllFiles()
        {
            return await transaction.ExecutionContext.Execute(() => adapter.GetFileAllIds(transaction).ToArray());
        }

        /// <summary>
        /// Удалить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        public async Task DeleteItem(string fileId)
        {
            await transaction.ExecutionContext.Execute(() =>
            {
                if (fileId == null) return;
                adapter.DeleteItem(transaction, fileId.ToLowerInvariant());
            });
        }

        /// <summary>
        /// Получить элемент.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns></returns>
        public async Task<StorageSizeCacheItem?> GetItem(string fileId)
        {
            return await transaction.ExecutionContext.Execute(() =>
            {
                if (fileId == null)
                {
                    return new StorageSizeCacheItem?();
                }
                var r = adapter.GetItem(transaction, fileId.ToLowerInvariant());
                if (r == null)
                {
                    return new StorageSizeCacheItem?();
                }
                return new StorageSizeCacheItem()
                {
                    Size = r.Value.Size,
                    Date = new DateTimeOffset(r.Value.DTicks, new TimeSpan(r.Value.OTicks))
                };
            });
        }

        /// <summary>
        /// Удалить все элементы.
        /// </summary>
        public async Task DeleteAllItems()
        {
            await transaction.ExecutionContext.Execute(() =>
            {
                adapter.DeleteAllItems(transaction);
            });
        }

        /// <summary>
        /// Проверка на существование.
        /// </summary>
        /// <param name="fileId">Идентификатор.</param>
        /// <returns>Результат.</returns>
        public async Task<bool> IsItemPresent(string fileId)
        {
            return await transaction.ExecutionContext.Execute(() =>
            {
                if (fileId == null)
                {
                    return false;
                }
                return adapter.IsItemPresent(transaction, fileId.ToLowerInvariant());
            });
        }

        /// <summary>
        /// Получить количество элементов.
        /// </summary>
        /// <returns>Количество элементов.</returns>
        public async Task<int> GetCount()
        {
            return await transaction.ExecutionContext.Execute(() => adapter.GetCount(transaction));
        }
    }
}