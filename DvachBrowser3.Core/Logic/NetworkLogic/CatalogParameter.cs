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
        public BoardCatalogLink Link;

        /// <summary>
        /// Режим обновления.
        /// </summary>
        public UpdateCatalogMode UpdateMode;
    }
}