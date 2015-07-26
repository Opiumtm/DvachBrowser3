using System;
using System.Windows.Input;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Режим фильтрации коллекции.
    /// </summary>
    public interface IPostCollectionFilterMode : IPageStateAware
    {
        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        IPostCollectionViewModel Parent { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Есть строка фильтра.
        /// </summary>
        bool HasFilterString { get; }

        /// <summary>
        /// Фильтр.
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Выбрать фильтр.
        /// </summary>
        void Select();

        /// <summary>
        /// Команда выбора фильтра.
        /// </summary>
        ICommand SelectCommand { get; }

        /// <summary>
        /// Фильтр выбран.
        /// </summary>
        event EventHandler Selected;

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>true, если нужно отобразить пост.</returns>
        bool FilterPost(IPostViewModel post);

        /// <summary>
        /// Скрывать отфильтрованные посты.
        /// </summary>
        bool HideFilteredPosts { get; set; }

        /// <summary>
        /// Фильтр по умолчанию.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Можно скрывать фильтрованные посты.
        /// </summary>
        bool CanHideFilteredPosts { get; }

        /// <summary>
        /// Список постов обновился.
        /// </summary>
        void PostListUpdated();
    }
}