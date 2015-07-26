using System;
using Windows.UI.Core;
using Windows.Web.Syndication;
using DvachBrowser3.Posts;

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
        public IPostViewModel Parent { get; private set; }

        /// <summary>
        /// Флаги.
        /// </summary>
        public PostFlags Flags
        {
            get { return flags; }
        }

        /// <summary>
        /// Сажа.
        /// </summary>
        public bool Sage
        {
            get { return (flags & PostFlags.Sage) != 0; }
        }

        /// <summary>
        /// Забанен.
        /// </summary>
        public bool Banned
        {
            get { return (flags & PostFlags.Banned) != 0; }
        }

        /// <summary>
        /// Прикреплённый.
        /// </summary>
        public bool Sticky
        {
            get { return (flags & PostFlags.Sticky) != 0; }
        }

        /// <summary>
        /// Тред закрыт.
        /// </summary>
        public bool Closed
        {
            get { return (flags & PostFlags.Closed) != 0; }
        }

        /// <summary>
        /// Превью треда.
        /// </summary>
        public bool ThreadPreview
        {
            get { return (flags & PostFlags.ThreadPreview) != 0; }
        }

        /// <summary>
        /// Редактирован.
        /// </summary>
        public bool IsEdited
        {
            get { return (flags & PostFlags.IsEdited) != 0; }
        }

        /// <summary>
        /// ОП-пост.
        /// </summary>
        public bool Op
        {
            get { return (flags & PostFlags.Op) != 0; }
        }

        /// <summary>
        /// Трипкод администратора.
        /// </summary>
        public bool AdminTrip
        {
            get { return (flags & PostFlags.AdminTrip) != 0; }
        }

        /// <summary>
        /// Мой пост.
        /// </summary>
        public bool MyPost
        {
            get { return Parent.Parent.CollectionSource.GetMyFlag(Parent.Data.Link); }
        }

        /// <summary>
        /// Флаги.
        /// </summary>
        private readonly PostFlags flags;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="flags">Флаги.</param>
        public PostFlagsViewModel(IPostViewModel parent, PostFlags flags)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            this.flags = flags;
            Parent = parent;
        }
    }
}