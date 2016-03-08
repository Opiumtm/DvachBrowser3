using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DvachBrowser3.Posting;
using DvachBrowser3.Storage;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
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
        /// <param name="file">Идентификатор.</param>
        public PostingMediaViewModel(IPostingMediaCollectionViewModel parent, PostingMediaFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            Parent = parent;
            Id = file.MediaFileId;
            OriginalName = file.OriginalName ?? "file.tmp";
            AppHelpers.DispatchAction(Initialize, false, 5);
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

        /// <summary>
        /// Оригинальное имя.
        /// </summary>
        public string OriginalName { get; }

        private bool addUniqueId;

        /// <summary>
        /// Добавлять уникальный ID.
        /// </summary>
        public bool AddUniqueId
        {
            get { return addUniqueId; }
            set
            {
                addUniqueId = value;
                RaisePropertyChanged();
                Flush();
            }
        }

        private bool resize;

        /// <summary>
        /// Изменять размер.
        /// </summary>
        public bool Resize
        {
            get { return resize; }
            set
            {
                resize = value;
                RaisePropertyChanged();
                Flush();
            }
        }

        private void Flush()
        {
            if (Parent?.Parent != null)
            {
                AppHelpers.ActionOnUiThread(async () =>
                {
                    await Parent.Parent.Flush(false);
                });
            }
        }

        /// <summary>
        /// Можно изменять размер.
        /// </summary>
        public bool CanResize
        {
            get
            {
                try
                {
                    var ext = Path.GetExtension(OriginalName);
                    if (".gif".Equals(ext, StringComparison.OrdinalIgnoreCase) || 
                        ".jpg".Equals(ext, StringComparison.OrdinalIgnoreCase) || 
                        ".jpeg".Equals(ext, StringComparison.OrdinalIgnoreCase) ||
                        ".png".Equals(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// Можно показывать предпросмотр.
        /// </summary>
        public bool CanPreview => CanResize;

        /// <summary>
        /// Создать описание файла.
        /// </summary>
        /// <returns>Описание файла.</returns>
        public PostingMediaFile CreateData()
        {
            return new PostingMediaFile()
            {
                OriginalName = OriginalName,
                AddUniqueId = AddUniqueId,
                Resize = Resize && CanResize,
                MediaFileId = Id
            };
        }

        /// <summary>
        /// Удалить файл.
        /// </summary>
        public async Task Delete()
        {
            if (Image != null)
            {
                var bimg = Image as BitmapImage;
                Image = null;
                HasImage = false;
                if (bimg != null)
                {
                    var makabaUri = new Uri("ms-appx:///Resources/MakabaLogo.png", UriKind.Absolute);
                    var f = await StorageFile.GetFileFromApplicationUriAsync(makabaUri);
                    using (var estr = await f.OpenAsync(FileAccessMode.Read))
                    {
                        await bimg.SetSourceAsync(estr);
                    }
                }
            }
            var storageService = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            await storageService.PostData.MediaStorage.DeleteMediaFile(Id);
            // ReSharper disable once UseNullPropagation
            if (Parent != null)
            {
                Parent.Media.Remove(this);
                AppHelpers.DispatchAction(async () =>
                {
                    await Parent.Parent.Flush(false);
                }, false, 0);
            }
        }

        private readonly Lazy<IStyleManager> styleManager = new Lazy<IStyleManager>(() => StyleManagerFactory.Current.GetManager());

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => styleManager.Value;

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