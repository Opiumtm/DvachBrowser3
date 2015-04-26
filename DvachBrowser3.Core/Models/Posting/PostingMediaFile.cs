﻿using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Медиафайл постинга.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostingMediaFile
    {
        /// <summary>
        /// Идентификатор медиа файла.
        /// </summary>
        [DataMember]
        public string MediaFileId { get; set; }

        /// <summary>
        /// Оригинальное имя.
        /// </summary>
        [DataMember]
        public string OriginalName { get; set; }
    }
}