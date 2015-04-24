using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Compression;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// База хранилища.
    /// </summary>
    public abstract class StorageBase : ServiceBase, IStorageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        protected StorageBase(IServiceProvider services, string folderName) : base(services)
        {
            FolderName = folderName;
        }

        /// <summary>
        /// Имя директории.
        /// </summary>
        protected string FolderName { get; private set; }

        /// <summary>
        /// Удалить старые данные.
        /// </summary>
        /// <param name="maxMb">Максимальное количество, Мб.</param>
        /// <returns>Таск.</returns>
        public virtual async Task Recycle(int maxMb)
        {
            await RecycleHelper.Recycle(await GetFolder(), maxMb);
        }

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual async Task Clear()
        {
            await RecycleHelper.Clear(await GetFolder());
        }

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        public virtual async Task<ulong> GetSize()
        {
            return await RecycleHelper.GetSize(await GetFolder());
        }

        /// <summary>
        /// Получить корневую директорию данных.
        /// </summary>
        /// <returns>Директория.</returns>
        protected async Task<StorageFolder> GetRootFolder()
        {
            return await ApplicationData.Current.LocalFolder.EnsureFolder("data");
        }

        /// <summary>
        /// Получить директорию.
        /// </summary>
        /// <returns>Директория.</returns>
        protected async Task<StorageFolder> GetFolder()
        {
            return await GetFolder(FolderName);
        }

        /// <summary>
        /// Получить директорию.
        /// </summary>
        /// <param name="folderName">Имя директории.</param>
        /// <returns>Директория.</returns>
        protected async Task<StorageFolder> GetFolder(string folderName)
        {
            return await (await GetRootFolder()).EnsureFolder(folderName);
        }

        /// <summary>
        /// Сохранить объект в файл.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Алгоритм сжатия.</param>
        /// <returns>Таск.</returns>
        protected async Task Save<T>(StorageFile file, T obj, CompressAlgorithm compress = CompressAlgorithm.NullAlgorithm)
        {
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();
            using (var tr = await file.OpenTransactedWriteAsync())
            {
                var str = new Compressor(tr.Stream, compress, 0);
                using (str)
                {
                    using (var wr = new StreamWriter(str.AsStreamForWrite(), Encoding.UTF8))
                    {
                        using (var xml = XmlWriter.Create(wr))
                        {
                            var x = xml;
                            await Task.Run(() => serializer.WriteObject(x, obj));
                        }
                    }                    
                }
                await tr.CommitAsync();
            }
        }

        /// <summary>
        /// Загрузить объект из файла.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="compress">Алгоритм сжатия.</param>
        /// <returns>Объект.</returns>
        protected async Task<T> Load<T>(StorageFile file, CompressAlgorithm compress = CompressAlgorithm.NullAlgorithm)
        {
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();
            using (var tr = await file.OpenSequentialReadAsync())
            {
                var str = compress == CompressAlgorithm.NullAlgorithm ? tr : new Decompressor(tr);
                try
                {
                    using (var rd = new StreamReader(str.AsStreamForRead(), Encoding.UTF8))
                    {
                        using (var xml = XmlReader.Create(rd))
                        {
                            var x = xml;
                            return await Task.Run(() => (T)serializer.ReadObject(x));
                        }
                    }
                }
                finally
                {
                    if (compress != CompressAlgorithm.NullAlgorithm)
                    {
                        str.Dispose();
                    }
                }
            }
        }
    }
}