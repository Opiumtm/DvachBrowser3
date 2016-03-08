using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

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
        /// <param name="token">Токен отмены.</param>
        /// <returns>Таск.</returns>
        public sealed override async Task<T> Complete(CancellationToken token)
        {
            var client = await CreateClient();
            T shortRes;
            if (ShortComplete(out shortRes))
            {
                return shortRes;
            }
            if (SetResultOnError)
            {
                try
                {
                    return await DoComplete(client, token);
                }
                catch (Exception ex)
                {
                    return OnError(ex);
                }
            }
            return await DoComplete(client, token);
        }

        /// <summary>
        /// Короткий результат.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <returns>true, если запрос делать не нужно.</returns>
        protected virtual bool ShortComplete(out T result)
        {
            result = default(T);
            return false;
        }

        /// <summary>
        /// Установить результат для ошибки.
        /// </summary>
        protected virtual bool SetResultOnError => false;

        /// <summary>
        /// Результат по ошибке.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Результат.</returns>
        protected virtual T OnError(Exception error)
        {
            return default(T);
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected abstract Task<T> DoComplete(HttpClient client, CancellationToken token);

        /// <summary>
        /// Создать HTTP-клиент.
        /// </summary>
        /// <returns>HTTP-клиент.</returns>
        protected virtual async Task<HttpClient> CreateClient()
        {
            var filter = GetHttpFilter();
            var result = new HttpClient(filter);
            await SetHeaders(result, filter);
            return result;
        }

        /// <summary>
        /// Получить фильтр HTTP.
        /// </summary>
        /// <returns>Фильтр HTTP.</returns>
        protected virtual IHttpFilter GetHttpFilter()
        {
            var filter = new HttpBaseProtocolFilter();
            filter.AutomaticDecompression = true;
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            filter.AllowUI = false;
            return filter;
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="filter">Фильтр.</param>
        /// <returns>Хидеры.</returns>
        protected virtual async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected abstract Uri GetRequestUri();

        protected ulong? totalReceive;

        /// <summary>
        /// Прогресс HTTP.
        /// </summary>
        /// <param name="progressInfo">Прогресс.</param>
        protected virtual void HttpOperationProgress(HttpProgress progressInfo)
        {
            var otherData = new HttpEngineProgressOtherData()
            {
                Kind = "HTTP",
                Progress = progressInfo
            };
            if (progressInfo.Stage == HttpProgressStage.ReceivingContent)
            {
                if (progressInfo.TotalBytesToReceive != null && progressInfo.TotalBytesToReceive > 0)
                {
                    totalReceive = progressInfo.TotalBytesToReceive;
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

        /// <summary>
        /// Прогресс загрузки.
        /// </summary>
        /// <param name="total">Всего.</param>
        /// <param name="progressInfo">Загружено.</param>
        protected virtual void DownloadProgress(ulong? total, ulong progressInfo)
        {
            var otherData = new EngineProgressOtherData()
            {
                Kind = "DOWNLOAD"
            };
            if (total != null && total > 0)
            {
                OnProgress(new EngineProgress(string.Format("Получено {0}/{1}", BytesToStr(progressInfo), BytesToStr(total.Value)), (double)progressInfo / (double)(total.Value) * 100.0, otherData));
            }
            else
            {
                OnProgress(new EngineProgress(string.Format("Получено {0}", BytesToStr(progressInfo)), null, otherData));
            }            
        }

        private const ulong Kb = 1024;

        private const ulong Mb = 1024*1024;

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
            //return bytes.ToString(CultureInfo.InvariantCulture);
            return string.Format("{0:F1} байт", (double)bytes / (double)Kb);
        }
    }

    /// <summary>
    /// Прогресс HTTP орперации.
    /// </summary>
    public class HttpEngineProgressOtherData : EngineProgressOtherData
    {
        /// <summary>
        /// Прогресс.
        /// </summary>
        public HttpProgress Progress { get; set; }
    }
}