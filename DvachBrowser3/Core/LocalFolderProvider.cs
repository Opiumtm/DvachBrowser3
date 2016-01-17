using System;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Storage;

namespace DvachBrowser3
{
    /// <summary>
    /// Провайдер директории хранилища.
    /// </summary>
    public sealed class LocalFolderProvider : ServiceBase, ILocalFolderProvider
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public LocalFolderProvider(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Получить хранилище.
        /// </summary>
        /// <returns>Хранилище.</returns>
        public Task<StorageFolder> GetFolder()
        {
            return Task.FromResult(ApplicationData.Current.LocalFolder);
        }
    }
}