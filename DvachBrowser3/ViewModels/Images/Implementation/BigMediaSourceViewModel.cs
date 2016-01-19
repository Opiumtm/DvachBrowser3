using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.System;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления больших постов.
    /// </summary>
    public sealed class BigMediaSourceViewModel : ImageSourceViewModelBase, IStartableViewModel, IBigMediaSourceViewModel
    {
        private readonly BoardLinkBase link;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public BigMediaSourceViewModel(BoardLinkBase link) : base(false)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            this.link = link;
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            var fn = linkTransform.GetMediaFileName(link);
            if (fn == null)
            {
                Extension = null;
            }
            else
            {
                Extension = fn.Split('.').LastOrDefault()?.ToLowerInvariant();
            }
            if (Extension == null)
            {
                SourceType = BigMediaSourceType.Static;
            }
            else
            {
                switch (Extension)
                {
                    case "gif":
                        SourceType = BigMediaSourceType.Gif;
                        break;
                    case "webm":
                        SourceType = BigMediaSourceType.Webm;
                        break;
                    default:
                        SourceType = BigMediaSourceType.Static;
                        break;
                }
            }
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="arg">Параметр.</param>
        /// <returns>Операция.</returns>
        protected override IEngineOperationsWithProgress<StorageFile, EngineProgress> OperationFactory(object arg)
        {
            return ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>().LoadMediaFile(link, LoadMediaFileMode.DefaultFullSize);
        }

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Start()
        {
            if (!ImageLoaded && !Load.Progress.IsActive)
            {
                Load.Start();
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        public Task Stop()
        {
            Load.Cancel();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Сохранить в файл.
        /// </summary>
        public async Task SaveToFile()
        {
            if (ImageLoaded)
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                var fn = linkTransform.GetMediaFileName(link);
                var f = await storage.FullSizeMediaFiles.GetFromMediaStorage(link);
                if (f == null)
                {
                    throw new FileNotFoundException();
                }
                var fsp = new FileSavePicker()
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                if (Extension != null)
                {
                    fsp.DefaultFileExtension = "." + Extension;
                }
                switch (Extension ?? "")
                {
                    case "jpg":
                    case "jpeg":
                        fsp.FileTypeChoices.Add("JPEG File", new[] { ".jpg", ".jpeg" });
                        break;
                    case "gif":
                        fsp.FileTypeChoices.Add("GIF File", new[] { ".gif" });
                        break;
                    case "png":
                        fsp.FileTypeChoices.Add("PNG File", new[] { ".png" });
                        break;
                    case "webm":
                        fsp.FileTypeChoices.Add("WEBM File", new[] { ".webm" });
                        break;
                    default:
                        fsp.FileTypeChoices.Add($"{Extension ?? ""} File", new[] { $".{Extension ?? ""}" });
                        break;
                }
                if (fn != null)
                {
                    fsp.SuggestedFileName = fn;
                }
                var toSave = await fsp.PickSaveFileAsync();
                if (toSave != null)
                {
                    //CachedFileManager.DeferUpdates(toSave);
                    await f.CopyAndReplaceAsync(toSave);
                    /*var status = await CachedFileManager.CompleteUpdatesAsync(toSave);
                    if (!(status == FileUpdateStatus.CompleteAndRenamed || status == FileUpdateStatus.Complete))
                    {
                        throw new IOException("Ошибка сохранения файла");
                    }*/
                }
            }
        }

        /// <summary>
        /// Открыть в браузере.
        /// </summary>
        public async Task OpenInBrowser()
        {
            var uri = link.GetWebLink();
            if (uri == null)
            {
                throw new InvalidOperationException("Невозможно получить URI файла");
            }
            await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Открыть в программе.
        /// </summary>
        public async Task OpenInProgram()
        {
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            var fn = linkTransform.GetMediaFileName(link);
            var f = await storage.FullSizeMediaFiles.GetFromMediaStorage(link);
            if (f == null)
            {
                throw new FileNotFoundException();
            }
            var f2 = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fn, CreationCollisionOption.GenerateUniqueName);
            await f.CopyAndReplaceAsync(f2);
            await Launcher.LaunchFileAsync(f2);
        }

        /// <summary>
        /// Скопировать в клипоборд.
        /// </summary>
        public async Task CopyToClipboard()
        {
            var dp = new DataPackage();
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var f = await storage.FullSizeMediaFiles.GetFromMediaStorage(link);
            dp.SetBitmap(RandomAccessStreamReference.CreateFromFile(f));
            Clipboard.SetContent(dp);
            Clipboard.Flush();
        }

        /// <summary>
        /// Расширение.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Тип изображения.
        /// </summary>
        public BigMediaSourceType SourceType { get; }

        /// <summary>
        /// Получить URL кэша изображения.
        /// </summary>
        /// <returns>URL кэша.</returns>
        protected override Uri GetImageCacheUri()
        {
            return ServiceLocator.Current.GetServiceOrThrow<IStorageService>().FullSizeMediaFiles.GetStoredImageUri(link);
        }
    }
}