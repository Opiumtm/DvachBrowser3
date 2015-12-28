using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиа-файл youtube.
    /// </summary>
    public sealed class YoutubeMediaFileViewModel : PostMediaFileViewModelBase<PostYoutubeVideo>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaData">Изображение.</param>
        public YoutubeMediaFileViewModel(IPostViewModel parent, PostYoutubeVideo mediaData) : base(parent, mediaData)
        {
            ThumbnailImage = new YoutubeThumbnailImageSourceViewModel(Link?.Engine, MediaData?.YoutubeVideoId);
        }

        /// <summary>
        /// Изображение предварительного просмотра.
        /// </summary>
        public override IImageSourceViewModelWithSize ThumbnailImage { get; }

        /// <summary>
        /// Высота.
        /// </summary>
        public override int? Height => null;

        /// <summary>
        /// Ширина.
        /// </summary>
        public override int? Width => null;

        /// <summary>
        /// Имя файла.
        /// </summary>
        public override string Name => "Видео YouTube";

        /// <summary>
        /// Строка с информацией.
        /// </summary>
        public override string InfoString => Name;
    }
}