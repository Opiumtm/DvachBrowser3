using System;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Провайдер экземпляра.
    /// </summary>
    internal interface IEsentInstanceProvider : IDisposable
    {
        /// <summary>
        /// Экземпляр.
        /// </summary>
        Instance Instance { get; }

        /// <summary>
        /// Инициализировать.
        /// </summary>
        Task Initialize();

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