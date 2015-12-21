using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Аргумент.
    /// </summary>
    public struct BoardCatalogLoaderArgument
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link;

        /// <summary>
        /// Режим обновления.
        /// </summary>
        public BoardCatalogUpdateMode UpdateMode;
    }
}