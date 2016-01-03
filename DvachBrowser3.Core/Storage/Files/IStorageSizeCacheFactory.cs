using System.Threading.Tasks;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Фабрика кэша размеров файлов.
    /// </summary>
    public interface IStorageSizeCacheFactory
    {
        /// <summary>
        /// Получить кэш.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="readOnly">Только для чтения.</param>
        /// <returns>Кэш.</returns>
        IStorageSizeCache Get(string id, bool readOnly);

        /// <summary>
        /// Инициализировать глобально.
        /// </summary>
        /// <returns>Таск.</returns>
        Task InitializeGlobal();

        /// <summary>
        /// Инициализировать кэш.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Результат.</returns>
        Task InitializeCache(string id);
    }
}