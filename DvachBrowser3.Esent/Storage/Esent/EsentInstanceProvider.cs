using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Storage.Files;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Экземпляр.
    /// </summary>
    internal sealed class EsentInstanceProvider : IEsentInstanceProvider
    {
        /// <summary>
        /// Экземпляр.
        /// </summary>
        public Instance Instance { get; private set; }

        private string databasePath;

        /// <summary>
        /// Инициализировать.
        /// </summary>
        public async Task Initialize()
        {
            var databaseDir = await GetDirectory();
            var fullPath = databaseDir.Path;
            databasePath = Path.Combine(fullPath, "database.edb");
            Instance = new Instance(databasePath)
            {
                Parameters =
                {
                    CreatePathIfNotExist = true,
                    TempDirectory = Path.Combine(fullPath, "temp"),
                    SystemDirectory = Path.Combine(fullPath, "system"),
                    LogFileDirectory = Path.Combine(fullPath, "logs"),
                    Recovery = true,
                    CircularLog = true,
                },
            };
            Instance.Init();
        }

        /// <summary>
        /// Получить директорию.
        /// </summary>
        /// <returns>Директория.</returns>
        public async Task<StorageFolder> GetDirectory()
        {
            var dataFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
            var indexFolder = await dataFolder.CreateFolderAsync("esent", CreationCollisionOption.OpenIfExists);
            return indexFolder;
        }

        /// <summary>
        /// Путь к базе данных.
        /// </summary>
        public string DatabasePath => databasePath;        

        /// <summary>
        /// Создать базу данных.
        /// </summary>
        /// <returns>Таск.</returns>
        public Task CreateDatabase()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var session = new Session(Instance))
                {
                    JET_DBID database;

                    Api.JetCreateDatabase(session, databasePath, null, out database, CreateDatabaseGrbit.None);
                    Api.JetCloseDatabase(session, database, CloseDatabaseGrbit.None);
                    Api.JetDetachDatabase(session, databasePath);
                }
            });
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Instance?.Dispose();
        }
    }
}