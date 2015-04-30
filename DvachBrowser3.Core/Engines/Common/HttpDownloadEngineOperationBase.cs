using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция загрузки по HTTP.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpDownloadEngineOperationBase<T, TParam> : HttpGetEngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметры.</param>
        /// <param name="services">Сервис.</param>
        protected HttpDownloadEngineOperationBase(TParam parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpResponseMessage message, CancellationToken token)
        {
            message.EnsureSuccessStatusCode();
            ulong length;
            var hasLength = message.Content.TryComputeLength(out length);
            var operation = message.Content.ReadAsInputStreamAsync();
            ulong? length1 = hasLength ? (ulong?) length : null;
            var progress = new Progress<ulong>(l => DownloadProgress(length1, l));
            using (var stream = await operation.AsTask(token))
            {
                return await DoComplete(message, stream.AsHttpCounting(progress), token);
            }
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="stream">Поток данных.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpResponseMessage message, IInputStream stream, CancellationToken token);

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