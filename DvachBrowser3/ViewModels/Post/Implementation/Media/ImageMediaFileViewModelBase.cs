using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовая модель представления изображения.
    /// </summary>
    /// <typeparam name="T">Тип медиа.</typeparam>
    public abstract class ImageMediaFileViewModelBase<T> : PostMediaFileViewModelBase<T> where T : PostImageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaData">Медиа данные.</param>
        protected ImageMediaFileViewModelBase(IPostViewModel parent, T mediaData) : base(parent, mediaData)
        {
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public override int? Height => MediaData?.Height;

        /// <summary>
        /// Ширина.
        /// </summary>
        public override int? Width => MediaData?.Width;

        /// <summary>
        /// Имя файла.
        /// </summary>
        public override string Name => MediaData?.Name;
    }
}