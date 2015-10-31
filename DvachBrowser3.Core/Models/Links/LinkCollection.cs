﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Коллекция ссылок.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(ThreadLinkCollection))]
    [KnownType(typeof(BoardLinkCollection))]
    public class LinkCollection
    {
        /// <summary>
        /// Ссылки.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> Links { get; set; }
    }
}