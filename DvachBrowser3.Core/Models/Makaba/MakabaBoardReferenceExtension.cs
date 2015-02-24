using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Board;

namespace DvachBrowser3.Makaba
{
    /// <summary>
    /// Расширение информации о борде для макабы.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class MakabaBoardReferenceExtension : BoardReferenceExtension
    {
        /// <summary>
        /// Иконки (null - нет иконок).
        /// </summary>
        [DataMember]
        public List<MakabaIconReference> Icons { get; set; }
    }
}