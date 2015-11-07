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
        /// Создать модель текста.
        /// </summary>
        /// <returns>Модель текста.</returns>
        protected override IPostTextViewModel CreateText()
        {
            return new PostTextViewModel(this, PostData);
        }
    }
}