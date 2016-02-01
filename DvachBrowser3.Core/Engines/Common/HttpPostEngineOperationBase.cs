using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP POST.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpPostEngineOperationBase<T, TParam> : HttpEngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected HttpPostEngineOperationBase(TParam parameter, IServiceProvider services)
            : base(parameter, services)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpClient client, CancellationToken token)
        {
            var uri = GetRequestUri();
            var content = await GetPostContent();
            var operation = client.PostAsync(uri, content).AsTask(token, new Progress<HttpProgress>(HttpOperationProgress));
            using (var message = await operation)
            {
                return await DoComplete(message, token);
            }
        }

        /// <summary>
        /// Получить контент для постинга.
        /// </summary>
        /// <returns>Контент.</returns>
        protected abstract Task<IHttpContent> GetPostContent();

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpResponseMessage message, CancellationToken token);
    }
}