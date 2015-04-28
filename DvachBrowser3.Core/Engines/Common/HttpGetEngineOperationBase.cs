using System;
using System.Threading.Tasks;
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
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpClient client)
        {
            var operation = client.GetAsync(GetRequestUri(), CompletionOption);
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