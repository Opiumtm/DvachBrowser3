using System.Threading.Tasks;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Базовый интерфейс хранилища.
    /// </summary>
    public interface IStorageBase
    {
        /// <summary>
        /// Удалить старые данные.
        /// </summary>
        /// <param name="maxMb">Максимальное количество, Мб.</param>
        /// <returns>Таск.</returns>
        Task Recycle(int maxMb);

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <returns>Таск.</returns>
        Task Clear();

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        Task<ulong> GetSize();         
    }
}