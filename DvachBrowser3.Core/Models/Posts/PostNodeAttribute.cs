using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Стандартный атрибут поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeAttribute : PostNodeAttributeBase
    {
        /// <summary>
        /// Атирибут.
        /// </summary>
        [DataMember]
        public PostNodeBasicAttribute Attribute { get; set; }
    }
}