using System;
using System.Windows.Input;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Режим фильтрации коллекции.
    /// </summary>
    public interface IPostCollectionFilterMode
    {
        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        IPostFiltering Parent { get; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        string Id { get; }

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
        /// Фильтр по умолчанию.
        /// </summary>
        bool IsDefault { get; }
    }
}