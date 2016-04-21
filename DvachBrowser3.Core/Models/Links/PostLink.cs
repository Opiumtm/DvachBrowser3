using System.IO;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на пост.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostLink : BoardLinkBase
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// Тред.
        /// </summary>
        [DataMember]
        public int Thread { get; set; }

        /// <summary>
        /// Пост.
        /// </summary>
        [DataMember]
        public int Post { get; set; }

        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.Post | BoardLinkKind.Thread;
        }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override BoardLinkBase DeepClone()
        {
            return new PostLink() { Board = Board, Thread = Thread, Post = Post, Engine = Engine };
        }
    }
}