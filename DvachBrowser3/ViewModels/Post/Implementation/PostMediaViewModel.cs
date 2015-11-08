using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиа файлы.
    /// </summary>
    public sealed class PostMediaViewModel : PostPartViewModelBase, IPostMediaViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="postData">Данные поста.</param>
        public PostMediaViewModel(IPostViewModel parent, PostTree postData) : base(parent)
        {
            var media = postData?.Media;
            if (media != null)
            {
                foreach (var model in media.Select(CreateModel).Where(model => model != null))
                {
                    Files.Add(model);
                }
            }

            PrimaryFile = Files.FirstOrDefault();
        }

        private IPostMediaFileViewModel CreateModel(PostMediaBase media)
        {
            if (media is PostYoutubeVideo)
            {
                return new YoutubeMediaFileViewModel(Parent, (PostYoutubeVideo)media);
            }
            if (media is PostImageWithThumbnail)
            {
                return new ImageMediaFileWithThumbnailViewModel(Parent, (PostImageWithThumbnail)media);
            }
            if (media is PostImage)
            {
                return new ImageMediaFileViewModel(Parent, (PostImage)media);
            }
            return null;
        }

        /// <summary>
        /// Файлы.
        /// </summary>
        public IList<IPostMediaFileViewModel> Files { get; } = new List<IPostMediaFileViewModel>();

        /// <summary>
        /// Основной файл.
        /// </summary>
        public IPostMediaFileViewModel PrimaryFile { get; }

        /// <summary>
        /// Имеется медиа.
        /// </summary>
        public bool HasMedia => PrimaryFile != null;
    }
}