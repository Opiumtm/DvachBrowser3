using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Базовый класс хранилища.
    /// </summary>
    public class StorageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        public StorageBase(IServiceProvider services, string folderName)
        {
            FolderName = folderName;
            Services = services;
        }

        /// <summary>
        /// Имя директории.
        /// </summary>
        public string FolderName { get; private set; }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }

        /// <summary>
        /// Получить корневую директорию.
        /// </summary>
        /// <returns>Корневая директорию.</returns>
        public virtual async Task<StorageFolder> GetRootDataFolder()
        {
            var folderProvider = Services.GetService<ILocalFolderProvider>();
            if (folderProvider != null)
            {
                return await (await folderProvider.GetFolder()).CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
            }
            return await ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить директорию данных.
        /// </summary>
        /// <returns>Директория данных.</returns>
        public async Task<StorageFolder> GetDataFolder()
        {
            return await (await GetRootDataFolder()).CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Сохранить строку.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Временная директория.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Таск.</returns>
        public async Task WriteString(StorageFile file, StorageFolder tempFolder, string obj, bool compress)
        {
            await file.ReplaceContent(tempFolder, async str =>
            {
                if (obj == null)
                {
                    return;
                }
                if (compress)
                {
                    using (var comp = new Compressor(str, CompressAlgorithm.Mszip, 0))
                    {
                        using (var wr = new StreamWriter(comp.AsStreamForWrite(), Encoding.UTF8))
                        {
                            await wr.FlushAsync();
                        }
                    }
                }
                else
                {
                    using (var wr = new StreamWriter(str.AsStream(), Encoding.UTF8))
                    {
                        await wr.WriteAsync(obj);
                        await wr.FlushAsync();
                    }
                }
            });
        }

        /// <summary>
        /// Сохранить объект в файл.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Временная директория.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Таск.</returns>
        public async Task WriteXmlObject<T>(StorageFile file, StorageFolder tempFolder, T obj, bool compress) where T: class
        {
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();
            await file.ReplaceContent(tempFolder, async str =>
            {
                if (obj == null)
                {
                    return;
                }
                if (compress)
                {
                    using (var comp = new Compressor(str, CompressAlgorithm.Mszip, 0))
                    {
                        await serializer.WriteObjectAsync(comp, obj);
                    }
                }
                else
                {
                    await serializer.WriteObjectAsync(str, obj);
                }
            });
        }

        private const int XmlReadBufferSize = 64*1024;

        /// <summary>
        /// Читать строку из файла.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Объект.</returns>
        public async Task<string> ReadString(StorageFile file, bool compress)
        {
            if (file == null)
            {
                return null;
            }
            return await file.PoliteRead(async str =>
            {
                if (str.Size == 0) return null;
                if (compress)
                {
                    using (var comp = new Decompressor(str))
                    {
                        using (var rd = new StreamReader(comp.AsStreamForRead(), Encoding.UTF8))
                        {
                            return await rd.ReadToEndAsync();
                        }
                    }
                }
                using (var rd = new StreamReader(str.AsStream(), Encoding.UTF8))
                {
                    return await rd.ReadToEndAsync();
                }
            }, TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Читать объект из файла.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Объект.</returns>
        public async Task<T> ReadXmlObject<T>(StorageFile file, bool compress) where T : class 
        {
            if (file == null)
            {
                return null;
            }
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();

            return await file.PoliteRead(async str =>
            {
                if (str.Size == 0) return null;
                if (compress)
                {
                    using (var comp = new Decompressor(str))
                    {
                        return (T)await serializer.ReadObjectAsync(comp);
                    }
                }
                return (T)await serializer.ReadObjectAsync(str);
            }, TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Сохранить медиа файл.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="originalFile">Оригинальный файл.</param>
        /// <param name="deleteOriginal">Удалить оригинальный файл.</param>
        /// <returns>Результат.</returns>
        public async Task WriteMediaFile(StorageFile file, StorageFile originalFile, bool deleteOriginal)
        {
            if (deleteOriginal)
            {
                bool ok;
                try
                {
                    await originalFile.MoveAndReplaceAsync(file);
                    ok = true;
                }
                catch (Exception)
                {
                    ok = false;
                }
                if (!ok)
                {
                    await originalFile.CopyAndReplaceAsync(file);
                    await originalFile.DeleteAsync();
                }
            }
            else
            {
                await originalFile.CopyAndReplaceAsync(file);
            }
        }

        /// <summary>
        /// Клонирование объекта.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="src">Исходный объект.</param>
        /// <returns>Результат.</returns>
        public async Task<T> DeepCloneObject<T>(T src) where T: class
        {
            if (src == null)
            {
                return null;
            }
            var cloneable = src as IDeepCloneable<T>;
            if (cloneable != null)
            {
                return cloneable.DeepClone();
            }
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();
            return (T)await serializer.DeepClone(src);
        }
    }
}