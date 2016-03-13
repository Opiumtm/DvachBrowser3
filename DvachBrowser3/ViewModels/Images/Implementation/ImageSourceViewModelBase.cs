using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DvachBrowser3.Engines;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовая модель источника изображений.
    /// </summary>
    public abstract class ImageSourceViewModelBase : ViewModelBase, IImageSourceViewModel, IWeakEventCallback
    {
        private ImageSource image;

        private readonly bool bindToPageLifetime;

        private IDisposable lifetimeToken;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="bindToPageLifetime">Привязать к времени жизни страницы.</param>
        protected ImageSourceViewModelBase(bool bindToPageLifetime = true)
        {
            this.bindToPageLifetime = bindToPageLifetime;
            LoadImpl = new StdEngineOperationWrapper<StorageFile>(OperationFactory) { NeedDispatch = true };
            LoadImpl.ResultGot += LoadImplOnResultGot;
            LoadImpl.Started += LoadImplOnStarted;
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == AppEvents.AppResumeId)
            {
                if (!ImageLoaded && !Load.Progress.IsActive)
                {
                    Load.Start();
                }
            }
        }

        private void LoadImplOnStarted(object sender, EventArgs eventArgs)
        {
            if (bindToPageLifetime)
            {
                Load.BindCancelToPageLifeTime();
                if (lifetimeToken == null)
                {
                    lifetimeToken = this.BindAppLifetimeEventsToPage();
                }
            }
        }

        /// <summary>
        /// Добавлять в памяти (null - автоматическое решение).
        /// </summary>
        protected virtual bool? UpdateInMem => null;

        private async Task<bool> CheckUpdateInMem(StorageFile file)
        {
            if (UpdateInMem != null)
            {
                return UpdateInMem.Value;
            }
            var prop = await file.GetBasicPropertiesAsync();
            if (prop.Size >= 256*1024)
            {
                return false;
            }
            return true;
        }

        private async Task DoLoadImplOnResultGot(object sender, StorageFile result)
        {
            try
            {
                if (result == null)
                {
                    Image = null;
                    return;
                }
                var cacheUri = GetImageCacheUri();
                if (SetImageSource)
                {
                    if (cacheUri == null || await CheckUpdateInMem(result))
                    {
                        var imgSource = new BitmapImage();
                        using (var s = new InMemoryRandomAccessStream())
                        {
                            using (var f = await result.OpenReadAsync())
                            {
                                await f.CopyStreamAsync(s, 4096);
                                s.Seek(0);
                                await imgSource.SetSourceAsync(s);
                            }
                        }
                        Image = imgSource;
                    }
                    else
                    {
                        var imgSource = new BitmapImage();
                        imgSource.UriSource = cacheUri;
                        Image = imgSource;
                    }
                }
                else
                {
                    // Пустое изображение.
                    Image = new BitmapImage();
                }
                // ReSharper disable once UseNullPropagation
                if (Image != null && lifetimeToken != null)
                {
                    lifetimeToken.Dispose();
                }
                ImageSourceGot?.Invoke(this, new ImageSourceGotEventArgs(cacheUri, result));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private void LoadImplOnResultGot(object sender, StorageFile result)
        {
            AppHelpers.DispatchAction(() => DoLoadImplOnResultGot(sender, result), false, 10);
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="arg">Параметр.</param>
        /// <returns>Операция.</returns>
        protected abstract IEngineOperationsWithProgress<StorageFile, EngineProgress> OperationFactory(object arg);

        /// <summary>
        /// Получить URL кэша изображения.
        /// </summary>
        /// <returns>URL кэша.</returns>
        protected virtual Uri GetImageCacheUri()
        {
            return null;
        }

        /// <summary>
        /// Изображение.
        /// </summary>
        public ImageSource Image
        {
            get { return image; }
            protected set
            {
                image = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged("ImageLoaded");
            }
        }

        /// <summary>
        /// Изображение загружено.
        /// </summary>
        public bool ImageLoaded => Image != null;

        /// <summary>
        /// Операция загрузки.
        /// </summary>
        protected StdEngineOperationWrapper<StorageFile> LoadImpl { get; }

        /// <summary>
        /// Операция загрузки.
        /// </summary>
        public IOperationViewModel Load => LoadImpl;

        /// <summary>
        /// Устанавливать значение изображения.
        /// </summary>
        public bool SetImageSource { get; set; } = true;

        /// <summary>
        /// Изображение получено.
        /// </summary>
        public event ImageSourceGotEventHandler ImageSourceGot;
    }
}