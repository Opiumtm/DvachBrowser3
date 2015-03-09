using System;
using System.Threading.Tasks;
using Windows.Storage;
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
            if (Parameter is BoardMediaLink || Parameter is MediaLink)
            {
                return Services.GetServiceOrThrow<IMakabaUriService>().GetBrowserLink(Parameter);
            }
            throw new ArgumentException("Неправильный формат ссылки (get media)");
        }

        /// <summary>
        /// Получить ответ.
        /// </summary>
        /// <param name="tempFile">Временный файл.</param>
        /// <param name="mimeType">Тип файла.</param>
        /// <returns>Результат.</returns>
        protected override async Task<IMediaResult> GetMediaResponse(StorageFile tempFile, string mimeType)
        {
            return new OperationResult()
            {
                MimeType = mimeType,
                TempFile = tempFile
            };
        }

        private class OperationResult : IMediaResult
        {
            public StorageFile TempFile { get; set; }

            public string MimeType { get; set; }
        }
    }
}