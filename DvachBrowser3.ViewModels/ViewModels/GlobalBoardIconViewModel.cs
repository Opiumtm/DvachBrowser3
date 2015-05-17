using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Глобальная модель иконки борды.
    /// </summary>
    public sealed class GlobalBoardIconViewModel : ViewModelBase, IIconWithNameViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaLink">Медиалинк.</param>
        /// <param name="autoLoad">Автозагрузка.</param>
        /// <param name="name">Имя.</param>
        public GlobalBoardIconViewModel(IPostViewModel parent, BoardLinkBase mediaLink, bool autoLoad, string name = null)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (mediaLink == null) throw new ArgumentNullException("mediaLink");
            this.Parent = parent;
            this.Name = name ?? "";
            this.mediaLink = mediaLink;
            var networkOperation = new NetworkOperationWrapper<ImageSourceResult, StorageFile>(OperationFactory, EventFactory, CancellationTokenFactory);
            NetworkOperation = networkOperation;
            networkOperation.SetResult += NetworkOperationOnSetResult;
            if (autoLoad)
            {
                networkOperation.ExecuteOperation();
            }
        }

        private void NetworkOperationOnSetResult(object sender, ImageSourceResult imageSourceResult)
        {
            Icon = imageSourceResult.Image;
            var bitmap = Icon as BitmapSource;
            if (bitmap != null)
            {
                Height = bitmap.PixelHeight;
                Width = bitmap.PixelWidth;
            }
            else
            {
                Height = 0;
                Width = 0;
            }
        }

        private CancellationToken CancellationTokenFactory()
        {
            return Parent.GetToken();
        }

        private async Task<ImageSourceResult> EventFactory(StorageFile storageFile)
        {
            return new ImageSourceResult(await storageFile.AsImageSourceAsync());
        }

        private IEngineOperationsWithProgress<StorageFile, EngineProgress> OperationFactory()
        {
            return NetworkLogic.LoadMediaFile(mediaLink, LoadMediaFileMode.DefaultSmallSize);
        }

        /// <summary>
        /// Линк медиа.
        /// </summary>
        private readonly BoardLinkBase mediaLink;

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostViewModel Parent { get; private set; }

        private ImageSource icon;

        /// <summary>
        /// Иконка.
        /// </summary>
        public ImageSource Icon
        {
            get { return icon; }
            private set
            {
                icon = value;
                OnPropertyChanged();
            }
        }

        public INetworkViewModel NetworkOperation { get; private set; }

        private int height;

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height
        {
            get { return height; }
            private set
            {
                height = value;
                OnPropertyChanged();
            }
        }

        private int width;

        /// <summary>
        /// Ширина.
        /// </summary>
        public int Width
        {
            get { return width; }
            private set
            {
                width = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; private set; }
    }
}