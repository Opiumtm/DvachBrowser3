using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Коллекция ссылок на борды.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardLinkCollection : LinkCollection
    {
        /// <summary>
        /// Информация о бордах.
        /// </summary>
        [DataMember]
        public Dictionary<string, FavoriteBoardInfo> BoardInfo { get; set; }
    }
}