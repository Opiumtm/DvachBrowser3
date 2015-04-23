using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на страницу борды.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class LinkCollection
    {
        /// <summary>
        /// Ссылки.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> Links { get; set; }
    }
}