using System;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// HTTP-операция.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class HttpEngineOperationBase<T, TParam> : EngineOperationBase<T, TParam>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected HttpEngineOperationBase(TParam parameter, IServiceProvider services)
            : base(parameter, services)
        {
        }


        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <returns>Таск.</returns>
        public sealed override async Task<T> Complete()
        {
            var client = await CreateClient();
            return await DoComplete(client);
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpClient client);

        /// <summary>
        /// Создать HTTP-клиент.
        /// </summary>
        /// <returns>HTTP-клиент.</returns>
        protected virtual async Task<HttpClient> CreateClient()
        {
            var result = new HttpClient();
            await SetHeaders(result);
            return result;
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <returns>Хидеры.</returns>
        protected virtual async Task SetHeaders(HttpClient client)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected abstract Uri GetRequestUri();

        /// <summary>
        /// Прогресс HTTP.
        /// </summary>
        /// <param name="asyncInfo">Асинхронная операция.</param>
        /// <param name="progressInfo">Прогресс.</param>
        protected virtual void HttpProgress(IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> asyncInfo, HttpProgress progressInfo)
        {
            dynamic otherData = new ExpandoObject();
            otherData.Kind = "HTTP";
            otherData.HttpProgress = progressInfo;
            if (progressInfo.Stage == HttpProgressStage.ReceivingContent)
            {
                if (progressInfo.TotalBytesToReceive != null && progressInfo.TotalBytesToReceive > 0)
                {
                    OnProgress(new EngineProgress(string.Format("Получено {0}/{1}", BytesToStr(progressInfo.BytesReceived), BytesToStr(progressInfo.TotalBytesToReceive.Value)), (double)progressInfo.BytesReceived / (double)(progressInfo.TotalBytesToReceive.Value) * 100.0, otherData));
                }
                else
                {
                    OnProgress(new EngineProgress(string.Format("Получено {0}", BytesToStr(progressInfo.BytesReceived)), null, otherData));
                }
            }
            else if (progressInfo.Stage == HttpProgressStage.SendingContent)
            {
                if (progressInfo.TotalBytesToSend != null && progressInfo.TotalBytesToSend > 0)
                {
                    OnProgress(new EngineProgress(string.Format("Отправлено {0}/{1}", BytesToStr(progressInfo.BytesSent), BytesToStr(progressInfo.TotalBytesToSend.Value)), (double)progressInfo.BytesSent / (double)(progressInfo.TotalBytesToSend.Value) * 100.0, otherData));
                }
                else
                {
                    OnProgress(new EngineProgress(string.Format("Отправлено {0}", BytesToStr(progressInfo.BytesSent)), null, otherData));
                }
            }
            else
            {
                OnProgress(new EngineProgress("Соединение с сервером", null, otherData));
            }
        }

        private const ulong Kb = 1024*1024;

        private const ulong Mb = 1024*1024*1024;

        /// <summary>
        /// Байты в строку.
        /// </summary>
        /// <param name="bytes">Байты.</param>
        /// <returns>Строка.</returns>
        protected string BytesToStr(ulong bytes)
        {
            if (bytes > Mb)
            {
                return string.Format("{0:F1} Мб", (double) bytes/(double) Mb);
            }
            if (bytes > Kb)
            {
                return string.Format("{0:F1} Кб", (double)bytes / (double)Kb);
            }
            return bytes.ToString(CultureInfo.InvariantCulture);
        }
    }
}