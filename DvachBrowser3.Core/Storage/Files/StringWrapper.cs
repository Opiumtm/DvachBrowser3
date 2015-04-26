using System.Runtime.Serialization;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Обёртка для строки.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class StringWrapper
    {
        /// <summary>
        /// Значение.
        /// </summary>
        [DataMember]
        public string Value { get; set; } 
    }
}