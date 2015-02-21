using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Текстовый узел поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostTextNode : PostNodeBase
    {
        /// <summary>
        /// Текст.
        /// </summary>
        [DataMember]
        public string Text { get; set; }                  
    }
}