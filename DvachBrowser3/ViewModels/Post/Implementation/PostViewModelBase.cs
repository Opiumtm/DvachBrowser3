using System;
using System.Collections.Generic;
using Windows.UI;
using DvachBrowser3.Configuration;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
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

        private IPostFlagsViewModel flags;

        /// <summary>
        /// Флаги.
        /// </summary>
        public IPostFlagsViewModel Flags => flags ?? (flags = CreateFlags());

        private IPostNameViewModel name;

        /// <summary>
        /// Имя.
        /// </summary>
        public IPostNameViewModel Name => name ?? (name = CreateName());

        /// <summary>
        /// Дата.
        /// </summary>
        public string Date
            => ServiceLocator.Current.GetServiceOrThrow<IUiConfigurationService>().UiPages.BoardSpecificDate
                ? PostData?.BoardSpecificDate
                : ServiceLocator.Current.GetServiceOrThrow<IDateService>().ToUserString(PostData?.Date ?? DateTime.MinValue);

        /// <summary>
        /// Номер поста.
        /// </summary>
        public int? PostNum => ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetPostNum(PostData?.Link);

        /// <summary>
        /// Номер поста.
        /// </summary>
        public string PostNumStr => "№" + ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetPostNumberDisplayString(PostData?.Link);

        private int? counter;

        /// <summary>
        /// Счётчик постов.
        /// </summary>
        public int? Counter
        {
            get { return counter; }
            set
            {
                counter = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Counter));
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(CounterStr));
            }
        }

        /// <summary>
        /// Счётчик постов.
        /// </summary>
        public string CounterStr => Counter != null ? Counter.ToString() : "";

        /// <summary>
        /// Цвет подложки изображения.
        /// </summary>
        public Color ImageBackgroundColor => BoardListBoardViewModelsHelper.GetDefaultBackgroundColor(PostData?.Link?.Engine);

        /// <summary>
        /// Количество пропущенных постов.
        /// </summary>
        public virtual string OmitPostCountStr => null;

        /// <summary>
        /// Дополнительные данные.
        /// </summary>
        public IDictionary<string, object> CustomAttachedData { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

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

        /// <summary>
        /// Создать модель флагов.
        /// </summary>
        /// <returns>Модель флагов.</returns>
        protected abstract IPostFlagsViewModel CreateFlags();

        /// <summary>
        /// Создать модель постера.
        /// </summary>
        /// <returns>Модель постера.</returns>
        protected abstract IPostNameViewModel CreateName();
    }
}