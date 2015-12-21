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
    public abstract class ImageSourceViewModelBase : ViewModelBase, IImageSourceViewModel
    {
        private ImageSource image;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="bindToPageLifetime">Привязать к времени жизни страницы.</param>
        protected ImageSourceViewModelBase(bool bindToPageLifetime = true)
        {
            LoadImpl = new StdEngineOperationWrapper<StorageFile>(OperationFactory) { NeedDispatch = true };
            LoadImpl.ResultGot += LoadImplOnResultGot;
            if (bindToPageLifetime)
            {
                Load.BindCancelToPageLifeTime();
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