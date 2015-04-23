using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Данные для постинга.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostingData
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Данные полей.
        /// </summary>
        [DataMember]
        public Dictionary<PostingFieldSemanticRole, object> FieldData { get; set; }

        /// <summary>
        /// Медиа файлы (идентификаторы файлов во временной директории).
        /// </summary>
        [DataMember]
        public string[] MediaFiles { get; set; }
    }
}