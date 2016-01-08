using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    public struct ThreadLoaderArgument
    {
        /// <summary>
        /// Ссылка на тред.
        /// </summary>
        public BoardLinkBase ThreadLink;

        /// <summary>
        /// Режим обновления.
        /// </summary>
        public ThreadLoaderUpdateMode UpdateMode;
    }
}