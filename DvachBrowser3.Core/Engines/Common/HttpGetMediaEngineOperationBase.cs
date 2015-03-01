using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP GET для медиа-файлов.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpGetMediaEngineOperationBase<T, TParam> : HttpGetEngineOperationBase<T, TParam>
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

        private ulong? totalBytes = null;

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpResponseMessage message)
        {
            message.EnsureSuccessStatusCode();
            ulong l;
            if (message.Content.TryComputeLength(out l))
            {
                totalBytes = l;
            }
            var tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid() + ".tmp", CreationCollisionOption.GenerateUniqueName);
            Exception error = null;
            var result = default(T);
            try
            {
                using (var str = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var operation = message.Content.WriteToStreamAsync(str);
                    operation.Progress = DownloadProgress;
                    Operation = operation;
                    try
                    {
                        await operation;
                    }
                    finally
                    {
                        Operation = null;
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
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }
                throw error;
            }
            return result;
        }

        /// <summary>
        /// Получить ответ.
        /// </summary>
        /// <param name="tempFile">Временный файл.</param>
        /// <param name="mimeType">Тип файла.</param>
        /// <returns>Результат.</returns>
        protected abstract Task<T> GetMediaResponse(StorageFile tempFile, string mimeType);

        private void DownloadProgress(IAsyncOperationWithProgress<ulong, ulong> asyncInfo, ulong progressInfo)
        {
            dynamic otherData = new ExpandoObject();
            otherData.Kind = "DOWNLOAD";
            if (totalBytes != null && totalBytes > 0)
            {
                OnProgress(new EngineProgress(string.Format("Получено {0}/{1}", BytesToStr(progressInfo), BytesToStr(totalBytes.Value)), (double)progressInfo / (double)(totalBytes.Value) * 100.0, otherData));
            }
            else
            {
                OnProgress(new EngineProgress(string.Format("Получено {0}", BytesToStr(progressInfo)), null, otherData));
            }
        }

        /// <summary>
        /// Прогресс HTTP.
        /// </summary>
        /// <param name="asyncInfo">Асинхронная операция.</param>
        /// <param name="progressInfo">Прогресс.</param>
        protected override void HttpProgress(IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> asyncInfo, HttpProgress progressInfo)
        {
            dynamic otherData = new ExpandoObject();
            otherData.Kind = "HTTP";
            otherData.HttpProgress = progressInfo;
            OnProgress(new EngineProgress("Соединение с сервером", null, otherData));
        }

        /// <summary>
        /// Опция определения.
        /// </summary>
        protected sealed override HttpCompletionOption CompletionOption
        {
            get
            {
                return HttpCompletionOption.ResponseHeadersRead;
            }
        }
    }
}