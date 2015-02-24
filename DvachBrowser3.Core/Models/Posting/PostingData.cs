using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Данные для постинга.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostingData
    {
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