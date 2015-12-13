using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3
{
    /// <summary>
    /// Хранилище любых данных.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public sealed class CustomDictionaryData
    {
        /// <summary>
        /// Данные.
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Data { get; set; }
    }
}