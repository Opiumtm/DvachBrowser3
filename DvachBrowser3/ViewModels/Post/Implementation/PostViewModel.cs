using DvachBrowser3.Posts;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public sealed class PostViewModel : PostViewModelBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="postData">Данные поста.</param>
        /// <param name="parent">Родительская модель.</param>
        public PostViewModel(PostTree postData, IPostCollectionViewModel parent) : base(postData, parent)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="postData">Данные поста.</param>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="omitPosts">Пропущено постов.</param>
        public PostViewModel(PostTree postData, IPostCollectionViewModel parent, int omitPosts) : base(postData, parent)
        {
            this.omitPosts = omitPosts;
        }

        /// <summary>
        /// Создать модель текста.
        /// </summary>
        /// <returns>Модель текста.</returns>
        protected override IPostTextViewModel CreateText()
        {
            return new PostTextViewModel(this, PostData);
        }

        /// <summary>
        /// Создать модель медиафайлов.
        /// </summary>
        /// <returns>Модель.</returns>
        protected override IPostMediaViewModel CreateMedia()
        {
            return new PostMediaViewModel(this, PostData);
        }

        /// <summary>
        /// Создать модель флагов.
        /// </summary>
        /// <returns>Модель флагов.</returns>
        protected override IPostFlagsViewModel CreateFlags()
        {
            return new PostFlagsViewModel(PostData, this);
        }

        /// <summary>
        /// Создать модель постера.
        /// </summary>
        /// <returns>Модель постера.</returns>
        protected override IPostNameViewModel CreateName()
        {
            return new PostNameViewModel(PostData, this);
        }

        /// <summary>
        /// Пропущено постов.
        /// </summary>
        private int? omitPosts;

        /// <summary>
        /// Количество пропущенных постов.
        /// </summary>
        public override string OmitPostCountStr => omitPosts?.ToString();
    }
}