using System.IO;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на тред.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadLink : BoardLinkBase
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
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.Thread;
        }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public override BoardLinkBase DeepClone()
        {
            return new ThreadLink() { Board = Board, Engine = Engine, Thread = Thread };
        }
    }
}