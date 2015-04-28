using DvachBrowser3.Links;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Параметр операции загрузки медиафайла.
    /// </summary>
    public struct LoadMediaFileOperationParameter
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link;

        /// <summary>
        /// Режим.
        /// </summary>
        public LoadMediaFileMode Mode;
    }
}