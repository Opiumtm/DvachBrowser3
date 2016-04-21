using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиа файл без предварительного просмотра.
    /// </summary>
    public sealed class ImageMediaFileWithThumbnailViewModel : ImageMediaFileViewModelBase<PostImageWithThumbnail>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaData">Данные медиа.</param>
        public ImageMediaFileWithThumbnailViewModel(IPostViewModel parent, PostImageWithThumbnail mediaData) : base(parent, mediaData)
        {
            ThumbnailImage = MediaData.Thumbnail != null ? new ThumbnailImageSourceViewModel(MediaData.Thumbnail) : null;
            ThumbnailImage?.SetImageCache(StaticImageCache.Thumbnails);
        }

        /// <summary>
        /// Изображение предварительного просмотра.
        /// </summary>
        public override IImageSourceViewModelWithSize ThumbnailImage { get; }
    }
}