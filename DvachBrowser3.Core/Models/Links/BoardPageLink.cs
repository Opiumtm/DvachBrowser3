using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на страницу борды.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardPageLink : BoardLinkBase
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// Страница.
        /// </summary>
        [DataMember]
        public int Page { get; set; }
    }
}