﻿using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Информация о любимой борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class FavoriteBoardInfo
    {
        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }
    }
}