using DvachBrowser3.Posts;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    public abstract class PostViewModelBase : ViewModelBase, IPostViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="postData">Данные поста.</param>
        /// <param name="parent">Родительская модель.</param>
        protected PostViewModelBase(PostTree postData, IPostCollectionViewModel parent)
        {
            PostData = postData;
            Parent = parent;
        }

        /// <summary>
        /// Пост.
        /// </summary>
        protected PostTree PostData { get; }

        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        public IPostCollectionViewModel Parent { get; }

        private IPostTextViewModel text;

        /// <summary>
        /// Текст поста.
        /// </summary>
        public IPostTextViewModel Text
        {
            get
            {
                if (text == null)
                {
                    text = CreateText();
                }
                return text;
            }
        }

        /// <summary>
        /// Создать модель текста.
        /// </summary>
        /// <returns>Модель текста.</returns>
        protected abstract IPostTextViewModel CreateText();
    }
}