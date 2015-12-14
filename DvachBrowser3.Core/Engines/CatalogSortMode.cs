namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Режим сортировки каталога.
    /// </summary>
    public enum CatalogSortMode : int
    {
        /// <summary>
        /// По умолчанию.
        /// </summary>
        Default = Bump,

        /// <summary>
        /// По бампам.
        /// </summary>
        Bump = 0,

        /// <summary>
        /// По времени создания треда.
        /// </summary>
        Created = 1,
    }
}