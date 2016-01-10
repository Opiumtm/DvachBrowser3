using System;
using Windows.Storage;
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

        private async void LoadImplOnResultGot(object sender, EventArgs e)
        {
            try
            {
                var result = LoadImpl.Result;
                if (result == null)
                {
                    Image = null;
                    return;
                }
                var cacheUri = GetImageCacheUri();
                if (cacheUri == null)
                {
                    var imgSource = new BitmapImage();
                    using (var f = await result.OpenReadAsync())
                    {
                        await imgSource.SetSourceAsync(f);
                    }
                    Image = imgSource;
                }
                else
                {
                    Image = new BitmapImage(cacheUri);
                }
                // ReSharper disable once UseNullPropagation
                if (Image != null && lifetimeToken != null)
                {
                    lifetimeToken.Dispose();
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
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
    }
}