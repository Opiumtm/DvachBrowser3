﻿using System;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using DvachBrowser3.Common;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка HTTP GET для строк JSON.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    /// <typeparam name="TJson">Тип объекта JSON.</typeparam>
    public abstract class HttpGetJsonEngineOperationBase<T, TParam, TJson> : HttpDownloadEngineOperationBase<T, TParam>
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
        /// <param name="stream">Поток данных.</param>
        /// <param name="size">Размер.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected override async Task<T> DoComplete(HttpResponseMessage message, IInputStream stream, ulong? size, CancellationToken token)
        {
            string etag = null;
            if (message.Headers.ContainsKey("ETag"))
            {
                etag = message.Headers["ETag"];
            }
            var serializer = Services.GetService<IJsonService>();
            var encoding = GetEncoding(message);
            TJson result;
            using (var buffer = new SemiBufferedStream(96*1024))
            {
                await stream.CopyToNetStreamWithProgress(buffer, new Progress<ulong>(l => DownloadProgress(size, l)), token);
                buffer.Position = 0;
                SignalProcessingJson();
                result = await DeserializeJson(serializer, buffer, encoding);
            }
            SignalProcessing();
            return await ProcessJson(result, etag, token);
        }

        private Task<TJson> DeserializeJson(IJsonService serializer, Stream buffer, Encoding encoding)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = serializer.Deserialize<TJson>(buffer, encoding);
                return result;
            });
        }

        /// <summary>
        /// Обработка данных.
        /// </summary>
        protected virtual void SignalProcessingJson()
        {
            var otherData = new HttpEngineProgressOtherData() { Kind = "PARSE" };
            OnProgress(new EngineProgress("Чтение JSON...", null, otherData));
        }

        /// <summary>
        /// Обработать результат JSON.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="etag">ETAG.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Результат.</returns>
        protected abstract Task<T> ProcessJson(TJson message, string etag, CancellationToken token);

        /// <summary>
        /// Получить кодировку ответа.
        /// </summary>
        /// <param name="msg">Сообщение.</param>
        /// <returns>Кодировка.</returns>
        protected virtual Encoding GetEncoding(HttpResponseMessage msg)
        {
            if (msg.Content.Headers.ContainsKey("Content-Type"))
            {
                var ct = msg.Content.Headers["Content-Type"];
                var regex =
                    Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(@"[^;]*;\s*charset=(?<charset>.*)$");
                var match = regex.Match(ct);
                if (match.Success)
                {
                    var ctt = match.Groups["charset"].Captures[0].Value.Trim();
                    return Encoding.GetEncoding(ctt);
                }
            }
            return Encoding.UTF8;
        }
    }
}