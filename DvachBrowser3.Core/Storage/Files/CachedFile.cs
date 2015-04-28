using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Файл с кэшированием в памяти.
    /// </summary>
    /// <typeparam name="T">Тип данных.</typeparam>
    public class CachedFile<T> where T : class
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parent">Родительское хранилище.</param>
        /// <param name="getFileTask">Функция получения файла.</param>
        /// <param name="getFolderTask">Функция получения директории.</param>
        /// <param name="compress">Сжимать файл.</param>
        public CachedFile(IServiceProvider services, StorageBase parent, Func<Task<StorageFile>> getFileTask, Func<Task<StorageFolder>> getFolderTask, bool compress)
        {
            Parent = parent;
            GetFileTask = getFileTask;
            Services = services;
            GetFolderTask = getFolderTask;
            Compress = compress;
        }

        /// <summary>
        /// Родительское хранилище.
        /// </summary>
        public StorageBase Parent { get; private set; }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }

        /// <summary>
        /// Значение в кэше.
        /// </summary>
        protected T CachedValue;

        /// <summary>
        /// Функция получения файла.
        /// </summary>
        protected Func<Task<StorageFile>> GetFileTask { get; private set; }

        /// <summary>
        /// Функция получения директории хранения.
        /// </summary>
        protected Func<Task<StorageFolder>> GetFolderTask { get; private set; }

        /// <summary>
        /// Сжимать файл.
        /// </summary>
        protected bool Compress { get; private set; }

        /// <summary>
        /// Сохранить значение.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task Save(T data)
        {
            try
            {
                Interlocked.Exchange(ref CachedValue, await Parent.DeepCloneObject(data));
                BackgroundSave(data);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }            
        }

        /// <summary>
        /// Сохранить значение синхронно.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveSync(T data)
        {
            try
            {
                Interlocked.Exchange(ref CachedValue, await Parent.DeepCloneObject(data));
                await Parent.WriteXmlObject(await GetFileTask(), await GetFolderTask(), data, Compress);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Сохранить значение в фоновом режиме.
        /// </summary>
        /// <param name="data">Данные.</param>
        protected void BackgroundSave(T data)
        {
            Task.Factory.StartNew(new Action(async () =>
            {
                try
                {
                    await Parent.WriteXmlObject(await GetFileTask(), await GetFolderTask(), data, Compress);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }));
        }


        /// <summary>
        /// Загрузить значение.
        /// </summary>
        /// <returns>Значение (null в случае отсутствия или ошибки).</returns>
        public async Task<T> Load()
        {
            try
            {
                var result = Interlocked.CompareExchange(ref CachedValue, null, null);
                if (result == null)
                {
                    result = await Parent.ReadXmlObject<T>(await GetFileTask(), Compress);
                    Interlocked.Exchange(ref CachedValue, await Parent.DeepCloneObject(result));
                }
                else
                {
                    result = await Parent.DeepCloneObject(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }            
        }

        /// <summary>
        /// Загрузить значение только для чтения.
        /// </summary>
        /// <returns>Значение (null в случае отсутствия или ошибки).</returns>
        public async Task<T> LoadForReadOnly()
        {
            try
            {
                var result = Interlocked.CompareExchange(ref CachedValue, null, null);
                if (result == null)
                {
                    result = await Parent.ReadXmlObject<T>(await GetFileTask(), Compress);
                    Interlocked.Exchange(ref CachedValue, result);
                }
                return result;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Инвалидировать кэш.
        /// </summary>
        public void InvalidateCache()
        {
            try
            {
                Interlocked.Exchange(ref CachedValue, null);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Очистить данные.
        /// </summary>
        public async Task Delete()
        {
            await (await GetFileTask()).DeleteAsync();
            Interlocked.Exchange(ref CachedValue, null);
        }
    }
}