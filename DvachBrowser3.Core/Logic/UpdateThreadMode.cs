﻿using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим обновления треда.
    /// </summary>
    [Flags]
    public enum UpdateThreadMode : int
    {
        /// <summary>
        /// Проверить штамп изменений.
        /// </summary>
        CheckETag = 0x0001,

        /// <summary>
        /// Частичная загрузка.
        /// </summary>
        Partial = 0x0002,

        /// <summary>
        /// Обновить данные посещения.
        /// </summary>
        UpdateVisitData = 0x0004,

        /// <summary>
        /// Сохранить в кэш.
        /// </summary>
        SaveToCache = 0x0008,

        /// <summary>
        /// По умолчанию.
        /// </summary>
        Default = CheckETag | Partial | UpdateVisitData | SaveToCache,

        /// <summary>
        /// По умолчанию частичный.
        /// </summary>
        DefaultPartial = CheckETag | Partial | UpdateVisitData | SaveToCache,

        /// <summary>
        /// По умолчанию полный.
        /// </summary>
        DefaultFull = CheckETag | UpdateVisitData | SaveToCache,

        /// <summary>
        /// По умолчанию для полной загрузки треда.
        /// </summary>
        DefaultReload = UpdateVisitData | SaveToCache,
    }
}