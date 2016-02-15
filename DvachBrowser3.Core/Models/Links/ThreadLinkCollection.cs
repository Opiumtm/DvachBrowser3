using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override LinkCollection DeepClone()
        {
            var r = new ThreadLinkCollection();
            if (Links != null)
            {
                r.Links = Links.Select(l => l?.DeepClone()).ToList();
            }
            if (ThreadInfo != null)
            {
                r.ThreadInfo = new Dictionary<string, ShortThreadInfo>();
                foreach (var kv in ThreadInfo)
                {
                    r.ThreadInfo[kv.Key] = kv.Value?.DeepClone();
                }
            }
            return r;
        }
    }
}