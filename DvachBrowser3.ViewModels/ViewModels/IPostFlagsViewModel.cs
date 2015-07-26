using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Флаги поста.
    /// </summary>
    public interface IPostFlagsViewModel
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Флаги.
        /// </summary>
        PostFlags Flags { get; }

        /// <summary>
        /// Сажа.
        /// </summary>
        bool Sage { get; }

        /// <summary>
        /// Забанен.
        /// </summary>
        bool Banned { get; }

        /// <summary>
        /// Прикреплённый.
        /// </summary>
        bool Sticky { get; }

        /// <summary>
        /// Тред закрыт.
        /// </summary>
        bool Closed { get; }

        /// <summary>
        /// Превью треда.
        /// </summary>
        bool ThreadPreview { get; }

        /// <summary>
        /// Редактирован.
        /// </summary>
        bool IsEdited { get; }

        /// <summary>
        /// ОП-пост.
        /// </summary>
        bool Op { get; }

        /// <summary>
        /// Трипкод администратора.
        /// </summary>
        bool AdminTrip { get; }

        /// <summary>
        /// Мой пост.
        /// </summary>
        bool MyPost { get; }
    }
}