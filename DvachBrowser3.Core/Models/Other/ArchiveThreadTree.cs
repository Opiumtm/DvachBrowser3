using System.Runtime.Serialization;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Other
{
    /// <summary>
    /// Ссылка на черновик поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ArchiveThreadTree : ThreadTree
    {
        /// <summary>
        /// Ссылка на архив.
        /// </summary>
        [DataMember]
        public ArchiveReference Reference { get; set; } 
    }
}