using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Board
{
    /// <summary>
    /// Ссылки на борды.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardReferences
    {
        /// <summary>
        /// Корневая ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase RootLink { get; set; }

        /// <summary>
        /// Ссылки.
        /// </summary>
        [DataMember]
        public List<BoardReference> References { get; set; }
    }
}