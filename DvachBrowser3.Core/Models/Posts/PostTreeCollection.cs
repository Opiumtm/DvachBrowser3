using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(ThreadTree))]
    [KnownType(typeof(ThreadPreviewTree))]
    public abstract class PostTreeCollection
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase ParentLink { get; set; }

        /// <summary>
        /// Посты.
        /// </summary>
        [DataMember]
        public List<PostTree> Posts { get; set; }


        /// <summary>
        /// Расширения.
        /// </summary>
        [DataMember]
        public List<PostTreeCollectionExtension> Extensions { get; set; }
    }
}