using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Данные по текущим постам.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class CurrentPostStoreData : IDeepCloneable<CurrentPostStoreData>
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

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public CurrentPostStoreData DeepClone()
        {
            var result = new CurrentPostStoreData();
            if (Threads != null)
            {
                result.Threads = new List<BoardLinkBase>();
                foreach (var t in Threads)
                {
                    result.Threads.Add(t?.DeepClone());
                }
            }
            if (Posts != null)
            {
                result.Posts = new Dictionary<string, BoardLinkBase>();
                foreach (var p in Posts)
                {
                    result.Posts[p.Key] = p.Value?.DeepClone();
                }
            }
            return result;
        }
    }
}