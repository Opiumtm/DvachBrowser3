﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Board
{
    /// <summary>
    /// Ссылка на борду.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardReference
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Короткое имя-идентификатор.
        /// </summary>
        [DataMember]
        public string ShortName { get; set; }

        /// <summary>
        /// Полное имя.
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Категория.
        /// </summary>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        /// Расширения.
        /// </summary>
        [DataMember]
        public List<BoardReferenceExtension> Extensions { get; set; }

        /// <summary>
        /// Взрослая борда.
        /// </summary>
        [DataMember]
        public bool IsAdult { get; set; }
    }
}