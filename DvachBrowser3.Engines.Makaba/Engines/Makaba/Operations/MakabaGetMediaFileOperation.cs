using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить медиафайл.
    /// </summary>
    public sealed class MakabaGetMediaFileOperation : HttpGetMediaEngineOperationBase<IMediaResult, BoardLinkBase>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetMediaFileOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            if (Parameter is BoardMediaLink)
            {
                return Services.GetServiceOrThrow<IMakabaUriService>().GetMediaLink(Parameter as BoardMediaLink);
            }
            if (Parameter is MediaLink)
            {
                return Services.GetServiceOrThrow<IMakabaUriService>().GetMediaLink(Parameter as MediaLink);
            }
            if (Parameter is YoutubeLink)
            {
                return Services.GetServiceOrThrow<IMakabaUriService>().GetMediaLink(Parameter as YoutubeLink);
            }
            throw new ArgumentException("Неправильный формат ссылки (get media)");
        }

        /// <summary>
        /// Получить ответ.
        /// </summary>
        /// <param name="tempFile">Временный файл.</param>
        /// <param name="mimeType">Тип файла.</param>
        /// <param name="token">Токен.</param>
        /// <returns>Результат.</returns>
        protected override async Task<IMediaResult> GetMediaResponse(StorageFile tempFile, string mimeType, CancellationToken token)
        {
            return new OperationResult()
            {
                MimeType = mimeType,
                TempFile = tempFile
            };
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client)
        {
            var p = Parameter as MediaLink;
            if (p != null)
            {
                if (p.IsAbsolute)
                {
                    return;
                }
            }
            await base.SetHeaders(client);
            await MakabaHeadersHelper.SetClientHeaders(Services, client);
        }

        private class OperationResult : IMediaResult
        {
            public StorageFile TempFile { get; set; }

            public string MimeType { get; set; }
        }
    }
}