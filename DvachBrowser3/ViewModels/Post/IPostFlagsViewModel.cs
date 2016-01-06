using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public interface IPostFlagsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; } 

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
        /// Закрыт.
        /// </summary>
        bool Closed { get; }

        /// <summary>
        /// Редактирован.
        /// </summary>
        bool IsEdited { get; }

        /// <summary>
        /// ОП.
        /// </summary>
        bool Op { get; }

        /// <summary>
        /// Трип администратора.
        /// </summary>
        bool AdminTrip { get; }

        /// <summary>
        /// Превью треда.
        /// </summary>
        bool ThreadPreview { get; }

        /// <summary>
        /// ОП-пост превью треда.
        /// </summary>
        bool ThreadPreviewOpPost { get; }
    }
}