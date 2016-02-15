using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override LinkCollection DeepClone()
        {
            var r = new BoardLinkCollection();
            if (Links != null)
            {
                r.Links = Links.Select(l => l?.DeepClone()).ToList();
            }
            if (BoardInfo != null)
            {
                r.BoardInfo = new Dictionary<string, FavoriteBoardInfo>();
                foreach (var kv in BoardInfo)
                {
                    r.BoardInfo[kv.Key] = kv.Value?.DeepClone();
                }
            }
            return r;
        }
    }
}