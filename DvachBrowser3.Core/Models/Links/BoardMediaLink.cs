using System.IO;
using System.Runtime.Serialization;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на медиафайл.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardMediaLink : BoardLinkBase
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// Относительный путь.
        /// </summary>
        [DataMember]
        public string RelativeUri { get; set; }

        /// <summary>
        /// Тип медиа.
        /// </summary>
        [DataMember]
        public MediaType MediaType { get; set; }

        /// <summary>
        /// Известный тип медиа.
        /// </summary>
        [DataMember]
        public KnownMediaType KnownMediaType { get; set; }

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
            return new BoardMediaLink() { Board = Board, KnownMediaType = KnownMediaType, Engine = Engine, MediaType = MediaType, RelativeUri = RelativeUri };
        }
    }
}