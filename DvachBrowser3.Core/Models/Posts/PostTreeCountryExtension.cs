using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Флаг (/po/ и некоторые другие борды).
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostTreeCountryExtension
    {
        /// <summary>
        /// Ссылка на иконку.
        /// </summary>
        [DataMember]
        public string Uri { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}