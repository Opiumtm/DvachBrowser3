using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Коллекция ссылок на треды.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadLinkCollection : LinkCollection
    {
        /// <summary>
        /// Информация о тредах.
        /// </summary>
        [DataMember]
        public Dictionary<string, ShortThreadInfo> ThreadInfo { get; set; }
    }
}