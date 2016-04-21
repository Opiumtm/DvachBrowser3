using System.IO;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на тэг тредов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadTagLink : BoardLinkBase
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// Тэг.
        /// </summary>
        [DataMember]
        public string Tag { get; set; }
        
        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.ThreadTag;
        }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override BoardLinkBase DeepClone()
        {
            return new ThreadTagLink() { Board = Board, Engine = Engine, Tag = Tag };
        }
    }
}