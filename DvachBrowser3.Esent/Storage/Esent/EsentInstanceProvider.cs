using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Storage.Files;
using Microsoft.Isam.Esent.Interop;
using Microsoft.Isam.Esent.Interop.Vista;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Экземпляр.
    /// </summary>
    internal sealed class EsentInstanceProvider : IEsentInstanceProvider
    {
        private string databasePath;

        private string fullPath;

        private Timer timer;

        private object sessionLock = new object();

        private int sessionCount;

        private Instance currentInstance;

        /// <summary>
        /// Инициализировать.
        /// </summary>
        public async Task Initialize()
        {
            var databaseDir = await GetDirectory();
            fullPath = databaseDir.Path;
            databasePath = Path.Combine(fullPath, "database.edb");
            timer = new Timer(TimerCallback, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
        }

        private void TimerCallback(object state)
        {
            lock (sessionLock)
            {
                if (sessionCount <= 0 && currentInstance != null)
                {
                    sessionCount = 0;
                    currentInstance.Dispose();
                    currentInstance = null;
                }
            }
        }

        private Instance CreateInstance()
        {
            var instance = new Instance(databasePath)
            {
                Parameters =
                {
                    CreatePathIfNotExist = true,
                    TempDirectory = Path.Combine(fullPath, "temp"),
                    SystemDirectory = Path.Combine(fullPath, "system"),
                    LogFileDirectory = Path.Combine(fullPath, "logs"),
                    Recovery = true,
                    CircularLog = true,
                    LogFileSize = 1024,
                },
            };
            instance.Init();
            return instance;
        }

        /// <summary>
        /// Получить экземпляр.
        /// </summary>
        public IEsentInstance GetInstance()
        {
            lock (sessionLock)
            {
                if (currentInstance == null)
                {
                    currentInstance = CreateInstance();
                }
                sessionCount++;
                return new EsentInstance(OnDispose, currentInstance);
            }
        }

        private void OnDispose()
        {
            lock (sessionLock)
            {
                sessionCount--;
            }
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
            if (File.Exists(databasePath))
            {
                return Task.FromResult(true);
            }
            return Task.Factory.StartNew(() =>
            {
                using (var instance = GetInstance())
                {
                    using (var session = new Session(instance.Instance))
                    {
                        JET_DBID database;

                        Api.JetCreateDatabase(session, databasePath, null, out database, CreateDatabaseGrbit.None);
                        Api.JetCloseDatabase(session, database, CloseDatabaseGrbit.None);
                        Api.JetDetachDatabase(session, databasePath);
                    }
                }
            });
        }
    }
}