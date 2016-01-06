using DvachBrowser3.Posts;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Флаги поста.
    /// </summary>
    public sealed class PostFlagsViewModel : ViewModelBase, IPostFlagsViewModel
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostViewModel Parent { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="parent">Родительская модель.</param>
        public PostFlagsViewModel(PostTree data, IPostViewModel parent)
        {
            Parent = parent;
            if (data != null)
            {
                Sage = (data.Flags & PostFlags.Sage) != 0;
                Banned = (data.Flags & PostFlags.Banned) != 0;
                Sticky = (data.Flags & PostFlags.Sticky) != 0;
                Closed = (data.Flags & PostFlags.Closed) != 0;
                IsEdited = (data.Flags & PostFlags.IsEdited) != 0;
                Op = (data.Flags & PostFlags.Op) != 0;
                AdminTrip = (data.Flags & PostFlags.AdminTrip) != 0;
                ThreadPreview = (data.Flags & PostFlags.ThreadPreview) != 0;
                ThreadPreviewOpPost = (data.Flags & PostFlags.ThreadPreviewOpPost) != 0;
            }
        }

        /// <summary>
        /// Сажа.
        /// </summary>
        public bool Sage { get; }

        /// <summary>
        /// Забанен.
        /// </summary>
        public bool Banned { get; }

        /// <summary>
        /// Прикреплённый.
        /// </summary>
        public bool Sticky { get; }

        /// <summary>
        /// Закрыт.
        /// </summary>
        public bool Closed { get; }

        /// <summary>
        /// Редактирован.
        /// </summary>
        public bool IsEdited { get; }

        /// <summary>
        /// ОП.
        /// </summary>
        public bool Op { get; }

        /// <summary>
        /// Трип администратора.
        /// </summary>
        public bool AdminTrip { get; }

        /// <summary>
        /// Превью треда.
        /// </summary>
        public bool ThreadPreview { get; }

        /// <summary>
        /// ОП-пост превью треда.
        /// </summary>
        public bool ThreadPreviewOpPost { get; }
    }
}