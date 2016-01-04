using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Провайдер экземпляра.
    /// </summary>
    internal interface IEsentInstanceProvider
    {
        /// <summary>
        /// Инициализировать.
        /// </summary>
        /// <returns></returns>
        Task Initialize();

        /// <summary>
        /// Получить экземпляр.
        /// </summary>
        IEsentInstance GetInstance();

        /// <summary>
        /// Получить директорию.
        /// </summary>
        /// <returns>Директория.</returns>
        Task<StorageFolder> GetDirectory();

        /// <summary>
        /// Путь к базе данных.
        /// </summary>
        string DatabasePath { get; }

        /// <summary>
        /// Создать базу данных.
        /// </summary>
        /// <returns>Таск.</returns>
        Task CreateDatabase();
    }
}