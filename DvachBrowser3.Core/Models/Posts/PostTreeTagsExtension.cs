using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Тэги.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostTreeTagsExtension : PostTreeExtension
    {
        /// <summary>
        /// Строка с тэгами.
        /// </summary>
        [DataMember]
        public string TagString { get; set; }

        /// <summary>
        /// Тэги.
        /// </summary>
        [DataMember]
        public List<string> Tags { get; set; }
    }
}