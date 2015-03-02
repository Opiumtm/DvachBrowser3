using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Board
{
    /// <summary>
    /// Информация о возможностях постинга.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardReferencePostingExtension : BoardReferenceExtension
    {
        /// <summary>
        /// Возможности постинга.
        /// </summary>
        [DataMember]
        public List<PostingCapability> Capabilities { get; set; }
    }
}