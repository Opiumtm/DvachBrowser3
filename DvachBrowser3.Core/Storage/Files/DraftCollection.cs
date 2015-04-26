using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Коллекция черновиков.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class DraftCollection
    {
        /// <summary>
        /// Черновики.
        /// </summary>
        [DataMember]
        public Dictionary<Guid, DraftReference> Drafts { get; set; }
    }
}