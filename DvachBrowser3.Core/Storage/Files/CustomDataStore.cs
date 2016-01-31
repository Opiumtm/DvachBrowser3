using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище любых данных.
    /// </summary>
    public class CustomDataStore : FolderStorage, ICustomDataStore
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="recycleConfig">Конфигурация очистки.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public CustomDataStore(IServiceProvider services, string folderName, CacheRecycleConfig recycleConfig, string cacheDescription) : base(services, folderName, recycleConfig, cacheDescription)
        {
        }

        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveCustomData(string key, Dictionary<string, object> data)
        {
            try
            {
                var fileName = GetFileName(key);
                var file = await GetCacheFile(fileName);
                var folder = await GetCacheFolder();
                await WriteCacheXmlObject(file, folder, new CustomDictionaryData() { Data = data }, true);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Загрузить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Данные.</returns>
        public async Task<Dictionary<string, object>> LoadCustomData(string key)
        {
            try
            {
                var fileName = GetFileName(key);
                var file = await GetCacheFileOrNull(fileName, true);
                if (file == null)
                {
                    return null;
                }
                return (await ReadXmlObject<CustomDictionaryData>(file, true))?.Data;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Удалить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Данне.</returns>
        public async Task DeleteCustomData(string key)
        {
            try
            {
                var fileName = GetFileName(key);
                var file = await GetCacheFile(fileName);
                await file.DeleteAsync(StorageDeleteOption.Default);
                await base.RemoveFromSizeCache(fileName);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Зарегистрировать тип.
        /// </summary>
        /// <param name="type">Тип.</param>
        public void RegisterType(Type type)
        {
            Services.GetServiceOrThrow<ISerializerCacheService>().RegisterCustomDataType(type);
        }

        private readonly Dictionary<string, string> hashIdCache = new Dictionary<string, string>();

        private string GetFileName(string key)
        {
            lock (hashIdCache)
            {
                if (hashIdCache.Count > 512)
                {
                    hashIdCache.Clear();
                }
                var id1 = (key ?? "").ToLowerInvariant();
                if (!hashIdCache.ContainsKey(id1))
                {
                    hashIdCache[id1] = UniqueIdHelper.CreateIdString(id1);
                }
                return $"{hashIdCache[id1]}.cache";
            }
        }
    }
}