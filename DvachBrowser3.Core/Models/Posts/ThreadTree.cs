using System.Runtime.Serialization;
using DvachBrowser3.Other;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Дерево постов в ветке.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(ArchiveThreadTree))]
    public class ThreadTree : PostTreeCollection
    {
        /// <summary>
        /// Время модификации.
        /// </summary>
        [DataMember]
        public string LastModified { get; set; }
    }
}