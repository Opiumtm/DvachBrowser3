﻿using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP HEAD.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpEngineHeadJsonEngineOperationBase<T, TParam> : HttpEngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected HttpEngineHeadJsonEngineOperationBase(TParam parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpClient client)
        {
            var operation = client.SendRequestAsync(new HttpRequestMessage(HttpMethod.Head, GetRequestUri()));
            operation.Progress = HttpProgress;
            Operation = operation;
            try
            {
                using (var message = await operation)
                {
                    return await DoComplete(message);
                }
            }
            finally
            {
                Operation = null;
            }
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpResponseMessage message);
    }
}