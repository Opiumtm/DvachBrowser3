using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Загрузка медиафайла.
    /// </summary>
    public class LoadMediaFileOperation : NetworkLogicOperation<StorageFile, LoadMediaFileOperationParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public LoadMediaFileOperation(IServiceProvider services, LoadMediaFileOperationParameter parameter)
            : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<StorageFile> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter.Link);
            var mediaStore = GetMediaStorage();
            var cache = (Parameter.Mode & LoadMediaFileMode.SaveToCache) != 0;
            var oldFile = await mediaStore.GetFromMediaStorage(Parameter.Link);
            if (oldFile != null)
            {
                return oldFile;
            }
            var newFile = await DownloadMediaFile(engine, token);
            if (newFile == null || newFile.TempFile == null)
            {
                throw new WebException("Неизвестная ошибки получения медиафайла");
            }
            var result = newFile.TempFile;
            if (cache)
            {
                result = await mediaStore.MoveToMediaStorage(Parameter.Link, result);
            }
            return result;
        }

        /// <summary>
        /// Получить хранилище медиафайлов.
        /// </summary>
        /// <returns>Хранилище медиафайлов.</returns>
        protected IMediaStorage GetMediaStorage()
        {
            if ((Parameter.Mode & LoadMediaFileMode.FullSizeMedia) != 0)
            {
                return Services.GetServiceOrThrow<IStorageService>().FullSizeMediaFiles;
            }
            return Services.GetServiceOrThrow<IStorageService>().SmallImages;
        }

        /// <summary>
        /// Семафор для загрузки маленьких изображений.
        /// </summary>
        protected static readonly MaxConcurrencyAccessManager<IMediaResult> ImageLoadAccessManager = new MaxConcurrencyAccessManager<IMediaResult>(CoreConstants.MaxParallelSmallImageDownloads);

        /// <summary>
        /// Загрузить медиафайл.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Медиафайл.</returns>
        protected async Task<IMediaResult> DownloadMediaFile(INetworkEngine engine, CancellationToken token)
        {
            var request = engine.GetMediaFile(Parameter.Link);
            request.Progress += (sender, e) => OnProgress(e);
            if ((Parameter.Mode & LoadMediaFileMode.FullSizeMedia) != 0)
            {
                token.ThrowIfCancellationRequested();
                return await request.Complete(token);
            }
            else
            {
                return await ImageLoadAccessManager.QueueAction(async () =>
                {
                    token.ThrowIfCancellationRequested();
                    return await request.Complete(token);
                });
            }
        }
    }
}