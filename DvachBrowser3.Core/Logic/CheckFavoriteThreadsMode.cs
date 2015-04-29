using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим проверки избранных тредов на обновления.
    /// </summary>
    [Flags]
    public enum CheckFavoriteThreadsMode : int
    {
        /// <summary>
        /// Асинхронная проверка.
        /// </summary>
        Async = 0x0001,

        /// <summary>
        /// Обновить данные в хранилище.
        /// </summary>
        UpdateData = 0x0002,

        /// <summary>
        /// Настройки по умолчанию.
        /// </summary>
        Default = Async | UpdateData
    }
}