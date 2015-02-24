using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Свойства капчи.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostingCaptchaCapability : PostingCapability
    {
        /// <summary>
        /// Типы капчи.
        /// </summary>
        [DataMember]
        public CaptchaTypes CaptchaTypes { get; set; }
    }
}