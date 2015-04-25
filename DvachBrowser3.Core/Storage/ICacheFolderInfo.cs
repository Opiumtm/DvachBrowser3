using System.Threading.Tasks;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Информация о директории кэша.
    /// </summary>
    public interface ICacheFolderInfo
    {
        /// <summary>
        /// Описание.
        /// </summary>
        string CacheDescription { get; }

        /// <summary>
        /// Получить размер кэша.
        /// </summary>
        /// <returns>Размер кэша.</returns>
        Task<ulong> GetCacheSize();

        /// <summary>
        /// Пересинхронизировать размер кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        Task ResyncCacheSize();

        /// <summary>
        /// Очистить кэш.
        /// </summary>
        /// <returns>Таск.</returns>
        Task ClearCache();

        /// <summary>
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        Task RecycleCache();
    }
}