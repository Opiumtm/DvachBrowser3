using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Поле комментария.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostingCommentCapability : PostingCapability
    {
        /// <summary>
        /// Тип разметки.
        /// </summary>
        [DataMember]
        public PostingMarkupType MarkupType { get; set; }

        /// <summary>
        /// Максимальный размер (null - неизвестно).
        /// </summary>
        [DataMember]
        public int? MaxLength { get; set; }
    }
}