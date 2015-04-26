using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Класс-помощник сериализации.
    /// </summary>
    public static class FileSerializationHelper
    {
        /// <summary>
        /// Сохранить объект асинхронно.
        /// </summary>
        /// <param name="serializer">Сериализатор.</param>
        /// <param name="wr">Средство записи XML.</param>
        /// <param name="graph">Объект.</param>
        /// <returns>Таск.</returns>
        public static Task WriteObjectAsync(this DataContractSerializer serializer, XmlWriter wr, object graph)
        {
            return Task.Factory.StartNew(() => serializer.WriteObject(wr, graph));
        }

        /// <summary>
        /// Прочитать объект асинхронно.
        /// </summary>
        /// <param name="serializer">Сериализатор.</param>
        /// <param name="rd">Средство чтения XML.</param>
        /// <returns>Таск.</returns>
        public static Task<object> ReadObjectAsync(this DataContractSerializer serializer, XmlReader rd)
        {
            return Task.Factory.StartNew(() => serializer.ReadObject(rd));
        }

        /// <summary>
        /// Читать файл при возможном параллельном изменении файла.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="action">Асинхронное действие.</param>
        /// <param name="timeout">Таймаут чтения.</param>
        /// <returns>Таск.</returns>
        public static async Task PoliteRead(this StorageFile file, Func<IRandomAccessStream, Task> action, TimeSpan timeout)
        {
            var bt = DateTime.Now;
            for (;;)
            {
                var str = await file.OpenAsync(FileAccessMode.Read);
                try
                {
                    await action(str);
                }
                catch
                {
                    if ((DateTime.Now - bt) > timeout)
                    {
                        throw;
                    }
                }
                finally
                {
                    str.Dispose();
                }
            }
        }

        /// <summary>
        /// Читать файл при возможном параллельном изменении файла.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="action">Асинхронное действие.</param>
        /// <param name="timeout">Таймаут чтения.</param>
        /// <returns>Таск.</returns>
        public static async Task<T> PoliteRead<T>(this StorageFile file, Func<IRandomAccessStream, Task<T>> action, TimeSpan timeout)
        {
            var bt = DateTime.Now;
            for (; ; )
            {
                var str = await file.OpenAsync(FileAccessMode.Read);
                try
                {
                    return await action(str);
                }
                catch
                {
                    if ((DateTime.Now - bt) > timeout)
                    {
                        throw;
                    }
                }
                finally
                {
                    str.Dispose();
                }
            }
        }

        /// <summary>
        /// Заменить содержимое файла.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Директория для временного файла.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public static async Task ReplaceContent(this StorageFile file, StorageFolder tempFolder, Func<IRandomAccessStream, Task> action)
        {
            var tempFile = await tempFolder.CreateFileAsync(file.Name, CreationCollisionOption.GenerateUniqueName);
            Exception error = null;
            try
            {
                using (var str = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await action(str);
                }
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        if (tempFile != null)
                        {
                            await tempFile.MoveAndReplaceAsync(file);
                            tempFile = null;                            
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (i == 4)
                        {
                            error = ex;
                            break;                            
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(0.25));
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            if (tempFile != null)
            {
                try
                {
                    await tempFile.DeleteAsync();
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
            if (error != null)
            {
                throw error;
            }
        }
    }
}