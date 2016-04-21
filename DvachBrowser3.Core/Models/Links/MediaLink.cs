using System.IO;
using System.Runtime.Serialization;
using DvachBrowser3.Posts;

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

        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.Media;
        }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override BoardLinkBase DeepClone()
        {
            return new MediaLink() { Engine = Engine, RelativeUri = RelativeUri, IsAbsolute = IsAbsolute };
        }
    }
}