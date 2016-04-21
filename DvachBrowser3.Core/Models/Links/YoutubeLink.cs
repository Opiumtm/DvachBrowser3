using System.IO;
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

        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.Youtube;
        }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override BoardLinkBase DeepClone()
        {
            return new YoutubeLink() {YoutubeId = YoutubeId, Engine = Engine};
        }
    }
}