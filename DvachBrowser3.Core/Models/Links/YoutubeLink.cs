using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на ютубу.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class YoutubeLink : BoardLinkBase
    {
        /// <summary>
        /// Идентификатор видео.
        /// </summary>
        [DataMember]
        public string YoutubeId { get; set; } 
    }
}