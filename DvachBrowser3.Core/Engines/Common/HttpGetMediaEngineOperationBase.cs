using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP GET для медиа-файлов.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpGetMediaEngineOperationBase<T, TParam> : HttpDownloadEngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected HttpGetMediaEngineOperationBase(TParam parameter, IServiceProvider services)
            : base(parameter, services)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="stream">Поток данных.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected override async Task<T> DoComplete(HttpResponseMessage message, IInputStream stream, CancellationToken token)
        {
            var tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid() + ".tmp", CreationCollisionOption.GenerateUniqueName);
            Exception error = null;
            try
            {
                using (var str = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var outStr = str.AsStream())
                    {
                        using (var inStr = stream.AsStreamForRead())
                        {
                            await inStr.CopyToAsync(outStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            if (error != null)
            {
                try
                {
                    await tempFile.DeleteAsync();
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
                throw error;
            }
            var contentType = message.Headers.ContainsKey("Content-Type") ? message.Headers["Content-Type"] : null;
            return await GetMediaResponse(tempFile, contentType, token);
        }

        /// <summary>
        /// Получить ответ.
        /// </summary>
        /// <param name="tempFile">Временный файл.</param>
        /// <param name="mimeType">Тип файла.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected abstract Task<T> GetMediaResponse(StorageFile tempFile, string mimeType, CancellationToken token);
    }
}