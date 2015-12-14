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
        Default = 0,

        /// <summary>
        /// По бампам.
        /// </summary>
        Bump = 1,

        /// <summary>
        /// По времени создания треда.
        /// </summary>
        Created = 2,
    }
}