using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим постинга.
    /// </summary>
    [Flags]
    public enum PostingMode : int
    {
        /// <summary>
        /// Удалить файлы для постинга из хранилища.
        /// </summary>
        DeleteFromStorage = 0x0001,

        /// <summary>
        /// Флаги по умолчанию.
        /// </summary>
        Default = DeleteFromStorage
    }
}