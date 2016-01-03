namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Элемент размера кэша.
    /// </summary>
    internal struct SizeCacheEsentItem
    {
        /// <summary>
        /// Идентификатор файла.
        /// </summary>
        public string FileId;

        /// <summary>
        /// Размер.
        /// </summary>
        public ulong Size;

        /// <summary>
        /// Первая часть даты.
        /// </summary>
        public long DTicks;

        /// <summary>
        /// Вторая часть даты.
        /// </summary>
        public long OTicks;
    }
}