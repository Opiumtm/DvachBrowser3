using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DvachBrowser3.Storage;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиафайл для постинга.
    /// </summary>
    public sealed class PostingMediaViewModel : ViewModelBase, IPostingMediaViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="id">Идентификатор.</param>
        public PostingMediaViewModel(IPostingMediaCollectionViewModel parent, string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            Parent = parent;
            Id = id;
            AppHelpers.DispatchAction(Initialize);
        }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostingMediaCollectionViewModel Parent { get; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; }

        private ImageSource image;

        /// <summary>
        /// Изображение.
        /// </summary>
        public ImageSource Image
        {
            get { return image; }
            private set
            {
                image = value;
                RaisePropertyChanged();
            }
        }

        private bool hasImage;

        /// <summary>
        /// Есть изображение.
        /// </summary>
        public bool HasImage
        {
            get { return hasImage; }
            private set
            {
                hasImage = value;
                RaisePropertyChanged();
            }
        }

        private string sizeStr;

        /// <summary>
        /// Строка с размером.
        /// </summary>
        public string SizeStr
        {
            get { return sizeStr; }
            private set
            {
                sizeStr = value;
                RaisePropertyChanged();
            }
        }

        private ulong fileSize;

        /// <summary>
        /// Размер файла.
        /// </summary>
        public ulong FileSize
        {
            get { return fileSize; }
            private set
            {
                fileSize = value;
                RaisePropertyChanged();
            }
        }

        private async Task Initialize()
        {
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            var mf = await storage.PostData.MediaStorage.GetMediaFile(Id);
            if (mf != null)
            {
                var bp = await mf.Value.File.GetBasicPropertiesAsync();
                FileSize = bp.Size;
                if (FileSize < 1024ul)
                {
                    SizeStr = $"{FileSize} байт";
                } else if (FileSize < 1024ul*1024ul)
                {
                    SizeStr = $"{(FileSize / 1024.0):F1} Кб";
                }
                else
                {
                    SizeStr = $"{(FileSize / (1024.0*1024.0)):F1} Мб";
                }
                Image = new BitmapImage(storage.PostData.MediaStorage.GetMediaFileUri(Id));
                HasImage = true;
            }
        }
    }
}