using DvachBrowser3.Links;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Параметр получения 
    /// </summary>
    public struct BoardReferencesParameter
    {
        /// <summary>
        /// Корневая ссылка.
        /// </summary>
        public BoardLinkBase RootLink;

        /// <summary>
        /// Форсировать обновление.
        /// </summary>
        public bool ForceUpdate;
    }
}