using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Информация об изображении.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadPictureInfo
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// Ширина.
        /// </summary>
        [DataMember]
        public int Width { get; set; }
    }
}