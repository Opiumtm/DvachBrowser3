using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Постер.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostTreePosterExtension : PostTreeExtension
    {
        /// <summary>
        /// Имя постера.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Трипкод.
        /// </summary>
        [DataMember]
        public string Tripcode { get; set; }
    }
}