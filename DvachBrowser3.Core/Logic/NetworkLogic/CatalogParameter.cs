using DvachBrowser3.Engines;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Параметр каталога.
    /// </summary>
    public struct CatalogParameter
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link;

        /// <summary>
        /// Режим сортировки.
        /// </summary>
        public CatalogSortMode SortMode;

        /// <summary>
        /// Режим обновления.
        /// </summary>
        public UpdateCatalogMode UpdateMode;
    }
}