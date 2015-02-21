using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Все узлы поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodes
    {
        /// <summary>
        /// Узы поста.
        /// </summary>
        [DataMember]
        public List<PostNode> Nodes { get; set; }
    }
}