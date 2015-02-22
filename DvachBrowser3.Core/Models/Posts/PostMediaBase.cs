using System.Runtime.Serialization;

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
        public int Size { get; set; }
    }
}