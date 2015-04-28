using DvachBrowser3.Links;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Параметр операции загрузки треда.
    /// </summary>
    public struct LoadThreadOperationParameter
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link;

        /// <summary>
        /// Режим.
        /// </summary>
        public UpdateThreadMode Mode;
    }
}