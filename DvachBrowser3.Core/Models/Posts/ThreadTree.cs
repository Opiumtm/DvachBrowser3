using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Дерево постов в ветке.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadTree : PostTreeCollection
    {
        /// <summary>
        /// Время модификации.
        /// </summary>
        [DataMember]
        public string LastModified { get; set; }
    }
}