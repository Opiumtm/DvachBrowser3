using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиа файл без предварительного просмотра.
    /// </summary>
    public sealed class ImageMediaFileViewModel : ImageMediaFileViewModelBase<PostImage>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaData">Данные медиа.</param>
        public ImageMediaFileViewModel(IPostViewModel parent, PostImage mediaData) : base(parent, mediaData)
        {
            ThumbnailImage = null;
        }

        /// <summary>
        /// Изображение предварительного просмотра.
        /// </summary>
        public override IImageSourceViewModelWithSize ThumbnailImage { get; }
    }
}