using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Коллекция ссылок.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(ThreadLinkCollection))]
    [KnownType(typeof(BoardLinkCollection))]
    public class LinkCollection : IDeepCloneable<LinkCollection>
    {
        /// <summary>
        /// Ссылки.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> Links { get; set; }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public virtual LinkCollection DeepClone()
        {
            var r = new LinkCollection();
            if (Links != null)
            {
                r.Links = Links.Select(l => l?.DeepClone()).ToList();
            }
            return r;
        }
    }
}