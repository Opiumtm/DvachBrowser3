using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Видео YouTube.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(PostImage))]
    public class PostYoutubeVideo : PostMediaBase
    {
        /// <summary>
        /// Видео с ютубы.
        /// </summary>
        [DataMember]
        public string YoutubeVideoId { get; set; }
    }
}