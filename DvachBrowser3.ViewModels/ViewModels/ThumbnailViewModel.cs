using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Предварительный просмотр.
    /// </summary>
    public sealed class ThumbnailViewModel : ViewModelBase, IThumbnailViewModel
    {
        /// <summary>
        /// Коснтруктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="media">Гиперссылка.</param>
        public ThumbnailViewModel(IPostViewModel parent, PostMediaBase media)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (media == null) throw new ArgumentNullException("media");
            Parent = parent;
            Media = media;
            hyperlink = new Lazy<string>(GetHyperlink);
            var withThumbnail = media as PostImageWithThumbnail;
            if (withThumbnail != null)
            {
                thumbnail = withThumbnail.Thumbnail;
            }
            var youtube = media as PostYoutubeVideo;
            if (youtube != null)
            {
                var ys = Services.GetServiceOrThrow<IYoutubeUriService>();
                thumbnail = new PostImage
                {
                    Link = new YoutubeLink {Engine = Media.Link.Engine, YoutubeId = youtube.YoutubeVideoId},
                    Size = null,
                    Name = null,
                    ParentLink = Parent.Data.Link,
                    Height = ys.ThumbnailHeight,
                    Width = ys.ThumbnailWidth
                };
            }
            if (thumbnail != null)
            {
                var networkOperation = new NetworkOperationWrapper<ImageSourceResult, StorageFile>(OperationFactory, EventFactory, CancellationTokenFactory);
                NetworkOperation = networkOperation;
                networkOperation.SetResult += NetworkOperationOnSetResult;
                HasThumbnailInMedia = true;
            }
            else
            {
                NetworkOperation = new EmptyNetworkOperationWrapper();
                HasThumbnailInMedia = false;
            }
        }

        private void NetworkOperationOnSetResult(object sender, ImageSourceResult imageSourceResult)
        {
            Image = imageSourceResult.Image;
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
            if (thumbnail == null)
            {
                throw new InvalidOperationException("Нет информации о превью медиафайла");
            }
            return NetworkLogic.LoadMediaFile(thumbnail.Link, LoadMediaFileMode.DefaultSmallSize);
        }

        private string GetHyperlink()
        {
            var engines = Services.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.GetEngineById(Media.Link.Engine);
            return engine.EngineUriService.GetBrowserLink(Media.Link).ToString();
        }

        private readonly Lazy<string> hyperlink;

        /// <summary>
        /// Гиперссылка.
        /// </summary>
        public string Hyperlink
        {
            get { return hyperlink.Value; }
        }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostViewModel Parent { get; private set; }

        /// <summary>
        /// Медиафайл.
        /// </summary>
        public PostMediaBase Media { get; private set; }

        /// <summary>
        /// Предпросмотр.
        /// </summary>
        private readonly PostImage thumbnail;

        private ImageSource image;

        /// <summary>
        /// Изображение.
        /// </summary>
        public ImageSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Ключ навигации для медиафайла.
        /// </summary>
        public INavigationKey MediaKey
        {
            get { return LinkKeyService.GetKey(Media.Link); }
        }

        /// <summary>
        /// Навигация к отображению медиафайла.
        /// </summary>
        public async void NavigateToMedia()
        {
            var y = Media as PostYoutubeVideo;
            if (y != null)
            {
                await Navigation.ViewYoutube(y.YoutubeVideoId);
            }
            else
            {
                Navigation.Navigate(ViewModelConstants.Pages.MediaView, MediaKey);
            }
        }

        /// <summary>
        /// Сетевая операция.
        /// </summary>
        public INetworkViewModel NetworkOperation { get; private set; }

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height
        {
            get
            {
                var m = Media as PostImageBase;
                if (m != null)
                {
                    return m.Height;
                }
                return 0;
            }
        }

        /// <summary>
        /// Ширина.
        /// </summary>
        public int Width
        {
            get
            {
                var m = Media as PostImageBase;
                if (m != null)
                {
                    return m.Width;
                }
                return 0;
            }
        }

        /// <summary>
        /// Высота предпросмотра.
        /// </summary>
        public int ThumbnailHeight
        {
            get { return thumbnail != null ? thumbnail.Height : 0; }
        }

        /// <summary>
        /// Ширина предпросмотра.
        /// </summary>
        public int ThumbnailWidth
        {
            get { return thumbnail != null ? thumbnail.Width : 0; }
        }

        /// <summary>
        /// Есть информация о размере.
        /// </summary>
        public bool HasSize
        {
            get { return Media.Size != null; }
        }

        /// <summary>
        /// Размер в килобайтах.
        /// </summary>
        public int SizeKb
        {
            get { return Media.Size ?? 0; }
        }

        /// <summary>
        /// Строка с размерами.
        /// </summary>
        public string SizeString
        {
            get
            {
                if (Media is PostYoutubeVideo)
                {
                    return "";
                }
                if (!HasSize)
                {
                    return "";
                }
                return string.Format("{0}Кб {1}x{2}", SizeKb, Width, Height);
            }
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name
        {
            get
            {
                var m = Media as PostImageBase;
                if (m != null)
                {
                    return m.Name;
                }
                var y = Media as PostYoutubeVideo;
                if (y != null)
                {
                    return "Видео YouTube";
                }
                return null;
            }
        }

        /// <summary>
        /// Есть предварительный просмотр в медиа.
        /// </summary>
        public bool HasThumbnailInMedia { get; private set; }
    }
}