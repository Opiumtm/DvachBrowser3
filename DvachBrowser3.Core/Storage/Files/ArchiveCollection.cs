using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Other;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Коллекция архивов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ArchiveCollection
    {
        /// <summary>
        /// Архивы.
        /// </summary>
        [DataMember]
        public Dictionary<Guid, ArchiveReference> Archives { get; set; }        
    }
}