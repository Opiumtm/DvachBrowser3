using System;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Конфигурация очистки кэша.
    /// </summary>
    public struct CacheRecycleConfig
    {
        /// <summary>
        /// Максимальное значение.
        /// </summary>
        public static readonly CacheRecycleConfig MaxValue = new CacheRecycleConfig()
        {
            MaxSize = ulong.MaxValue,
            NormalSize = ulong.MaxValue,
            MaxFiles = int.MaxValue,
            NormalFiles = int.MaxValue
        };

        /// <summary>
        /// Максимальный размер.
        /// </summary>
        public ulong MaxSize;

        /// <summary>
        /// Нормальный размер.
        /// </summary>
        public ulong NormalSize;

        /// <summary>
        /// Максимальное количество файлов.
        /// </summary>
        public int MaxFiles;

        /// <summary>
        /// Нормальное количество файлов.
        /// </summary>
        public int NormalFiles;

    }
}