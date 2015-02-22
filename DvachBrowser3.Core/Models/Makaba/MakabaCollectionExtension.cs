using System.Runtime.Serialization;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Makaba
{
    /// <summary>
    /// Расширение коллекции Makaba.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class MakabaCollectionExtension : PostTreeCollectionExtension
    {
        /// <summary>
        /// Данные Makaba.
        /// </summary>
        [DataMember]
        public MakabaEntityTree Entity { get; set; } 
    }
}