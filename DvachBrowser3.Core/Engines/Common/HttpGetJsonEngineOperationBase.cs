using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP GET для строк JSON.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    /// <typeparam name="TJson">Тип объекта JSON.</typeparam>
    public abstract class HttpGetJsonEngineOperationBase<T, TParam, TJson> : HttpGetEngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected HttpGetJsonEngineOperationBase(TParam parameter, IServiceProvider services)
            : base(parameter, services)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected sealed override async Task<T> DoComplete(HttpResponseMessage message)
        {
            message.EnsureSuccessStatusCode();
            string etag = null;
            if (message.Headers.ContainsKey("ETag"))
            {
                etag = message.Headers["ETag"];
            }
            var str = await message.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TJson>(str);
            Operation = null;
            SignalProcessing();
            return await ProcessJson(result, etag);
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <returns>Результат.</returns>
        protected abstract Task<T> ProcessJson(TJson message, string etag);

        /// <summary>
        /// Опция определения.
        /// </summary>
        protected sealed override HttpCompletionOption CompletionOption
        {
            get
            {
                return HttpCompletionOption.ResponseContentRead;
            }
        }
    }
}