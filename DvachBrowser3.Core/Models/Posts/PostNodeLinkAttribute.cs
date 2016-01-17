using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Ссылка.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeLinkAttribute : PostNodeAttributeBase
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public string LinkUri { get; set; }
    }
}