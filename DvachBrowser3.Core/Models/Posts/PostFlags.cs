using System;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Флаги поста.
    /// </summary>
    [Flags]
    public enum PostFlags : int
    {
        /// <summary>
        /// Сажа.
        /// </summary>
        Sage = 0x0001,

        /// <summary>
        /// Постер забанен.
        /// </summary>
        Banned = 0x0002,

        /// <summary>
        /// Прикреплённый пост.
        /// </summary>
        Sticky = 0x0004,

        /// <summary>
        /// Тема закрыта.
        /// </summary>
        Closed = 0x0008,

        /// <summary>
        /// Предварительный просмотр треда.
        /// </summary>
        ThreadPreview = 0x0010,

        /// <summary>
        /// Пост редактирован.
        /// </summary>
        IsEdited = 0x0020,

        /// <summary>
        /// ОП.
        /// </summary>
        Op = 0x0040,

        /// <summary>
        /// Трипкод администратора.
        /// </summary>
        AdminTrip = 0x0080,
    }
}