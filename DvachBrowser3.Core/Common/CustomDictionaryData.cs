using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3
{
    /// <summary>
    /// Хранилище любых данных.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(BoardLinkBase))]
    public sealed class CustomDictionaryData
    {
        /// <summary>
        /// Данные.
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Data { get; set; }
    }
}