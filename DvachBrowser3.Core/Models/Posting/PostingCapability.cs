using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Поле для постинга.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(PostingMediaFileCapability))]
    [KnownType(typeof(PostingCaptchaCapability))]
    [KnownType(typeof(PostingIconCapability))]
    [KnownType(typeof(PostingCommentCapability))]
    public class PostingCapability
    {
        /// <summary>
        /// Семантическая роль.
        /// </summary>
        [DataMember]
        public PostingFieldSemanticRole Role { get; set; }
    }
}