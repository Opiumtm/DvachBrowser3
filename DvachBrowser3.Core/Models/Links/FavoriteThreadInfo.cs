using System.Runtime.Serialization;
using DvachBrowser3.Other;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Информация об избранном треде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class FavoriteThreadInfo : ShortThreadInfo
    {
        /// <summary>
        /// Информация о количестве постов.
        /// </summary>
        [DataMember]
        public PostCountInfo CountInfo { get; set; }
    }
}