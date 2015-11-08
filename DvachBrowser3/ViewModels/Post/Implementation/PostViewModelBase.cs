using DvachBrowser3.Links;
using DvachBrowser3.Posts;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовый класс модели представления поста.
    /// </summary>
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

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link => PostData?.Link;

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        public BoardLinkBase ParentLink => PostData?.ParentLink;

        private IPostTextViewModel text;

        /// <summary>
        /// Текст поста.
        /// </summary>
        public IPostTextViewModel Text => text ?? (text = CreateText());

        private IPostMediaViewModel media;

        /// <summary>
        /// Медиафайлы.
        /// </summary>
        public IPostMediaViewModel Media => media ?? (media = CreateMedia());

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Subject => PostData?.Subject ?? "";

        /// <summary>
        /// Создать модель текста.
        /// </summary>
        /// <returns>Модель.</returns>
        protected abstract IPostTextViewModel CreateText();

        /// <summary>
        /// Создать модель медиафайлов.
        /// </summary>
        /// <returns>Модель.</returns>
        protected abstract IPostMediaViewModel CreateMedia();
    }
}