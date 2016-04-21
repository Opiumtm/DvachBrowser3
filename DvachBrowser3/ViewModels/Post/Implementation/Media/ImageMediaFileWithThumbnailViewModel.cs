using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// ����� ���� ��� ���������������� ���������.
    /// </summary>
    public sealed class ImageMediaFileWithThumbnailViewModel : ImageMediaFileViewModelBase<PostImageWithThumbnail>
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="parent">������������ ������.</param>
        /// <param name="mediaData">������ �����.</param>
        public ImageMediaFileWithThumbnailViewModel(IPostViewModel parent, PostImageWithThumbnail mediaData) : base(parent, mediaData)
        {
            ThumbnailImage = MediaData.Thumbnail != null ? new ThumbnailImageSourceViewModel(MediaData.Thumbnail) : null;
            ThumbnailImage?.SetImageCache(StaticImageCache.Thumbnails);
        }

        /// <summary>
        /// ����������� ���������������� ���������.
        /// </summary>
        public override IImageSourceViewModelWithSize ThumbnailImage { get; }
    }
}