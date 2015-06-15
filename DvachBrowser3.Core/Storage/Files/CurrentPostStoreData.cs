using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Данные по текущим постам.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class CurrentPostStoreData
    {
        /// <summary>
        /// Треды.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> Threads { get; set; }

        /// <summary>
        /// Посты.
        /// </summary>
        [DataMember]
        public Dictionary<string, BoardLinkBase> Posts { get; set; }
    }
}