using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Медиа файл.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(PostImageBase))]
    [KnownType(typeof(PostYoutubeVideo))]
    public abstract class PostMediaBase
    {
        /// <summary>
        /// Размер.
        /// </summary>
        [DataMember]
        public int? Size { get; set; }

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
    }
}