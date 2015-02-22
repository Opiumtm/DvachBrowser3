using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на медиафайл.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class MediaLink : BoardLinkBase
    {
        [DataMember]
        public bool IsAbsolute { get; set; }

        /// <summary>
        /// Относительный путь.
        /// </summary>
        [DataMember]
        public string RelativeUri { get; set; }         
    }
}