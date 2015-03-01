using System;
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
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpClient client)
        {
            using (var content = await GetPostContent())
            {
                var operation = client.PostAsync(GetRequestUri(), content);
                operation.Progress = HttpProgress;
                using (var message = await operation)
                {
                    return await DoComplete(message);
                }                
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
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpResponseMessage message);
    }
}