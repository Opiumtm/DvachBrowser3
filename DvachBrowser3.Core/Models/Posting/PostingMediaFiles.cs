using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Медиафайлы.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostingMediaFiles
    {
        /// <summary>
        /// Файлы.
        /// </summary>
        [DataMember]
        public List<PostingMediaFile> Files { get; set; }
    }
}