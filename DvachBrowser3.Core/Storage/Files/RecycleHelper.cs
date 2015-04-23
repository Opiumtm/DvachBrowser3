using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Класс-помощник перечисления данных.
    /// </summary>
    public static class RecycleHelper
    {
        /// <summary>
        /// Создать директорию, если не создана.
        /// </summary>
        /// <param name="parent">Родитель.</param>
        /// <param name="name">Имя.</param>
        /// <returns>Директория.</returns>
        public static async Task<StorageFolder> EnsureFolder(this StorageFolder parent, string name)
        {
            return await parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить размер директории.
        /// </summary>
        /// <param name="folder">Директория.</param>
        /// <returns>Размер.</returns>
        public static async Task<ulong> GetSize(StorageFolder folder)
        {
            var props = await folder.GetBasicPropertiesAsync();
            return props.Size;
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        /// <param name="folder">Директория.</param>
        /// <returns>Таск.</returns>
        public static async Task Clear(StorageFolder folder)
        {
            await folder.DeleteAsync();
        }

        /// <summary>
        /// Удалить старые данные.
        /// </summary>
        /// <param name="folder">Директория.</param>
        /// <param name="maxMb">Максимальное количество мегабайт.</param>
        /// <returns>Таск.</returns>
        public static async Task Recycle(StorageFolder folder, int maxMb)
        {
            var totalSize = await GetSize(folder);
            ulong maxSize = ((ulong)maxMb)*1024*1024;
            if (totalSize <= maxSize)
            {
                return;
            }
            var files = (await folder.GetFilesAsync(CommonFileQuery.OrderByDate)).ToArray();
            foreach (var f in files)
            {
                if (totalSize <= maxSize)
                {
                    return;
                }
                try
                {
                    var props = await f.GetBasicPropertiesAsync();
                    var sz = props.Size;
                    await f.DeleteAsync();
                    totalSize -= sz;
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception ex)
                {
                    // Игнорируем ошибки
                }
            }
        }
    }
}