using DvachBrowser3.Links;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Параметр операции загрузки страницы борды.
    /// </summary>
    public struct LoadBoardPageOperationParameter
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link;

        /// <summary>
        /// Режим.
        /// </summary>
        public UpdateBoardPageMode Mode;
    }
}