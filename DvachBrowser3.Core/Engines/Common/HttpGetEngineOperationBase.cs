﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP GET.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpGetEngineOperationBase<T, TParam> : HttpEngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected HttpGetEngineOperationBase(TParam parameter, IServiceProvider services)
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
            var operation = client.GetAsync(GetRequestUri(), CompletionOption);
            using (var message = await operation.AsTask(token, new Progress<HttpProgress>(HttpOperationProgress)))
            {
                return await DoComplete(message, token);                
            }
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpResponseMessage message, CancellationToken token);

        /// <summary>
        /// Опция определения.
        /// </summary>
        protected virtual HttpCompletionOption CompletionOption
        {
            get
            {
                return HttpCompletionOption.ResponseContentRead;
            }
        }
    }
}