using System;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Ссылка на черновик поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class DraftReference
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [DataMember]
        public string DraftTitle { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [DataMember]
        public DateTime DraftDate { get; set; }
    }
}