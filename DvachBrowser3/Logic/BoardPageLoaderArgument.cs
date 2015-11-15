using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Аргумент.
    /// </summary>
    public struct BoardPageLoaderArgument
    {
        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        public BoardLinkBase PageLink;

        /// <summary>
        /// Режим обновления.
        /// </summary>
        public BoardPageLoaderUpdateMode UpdateMode;
    }
}