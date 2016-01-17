using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Ссылка на борду.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeBoardLinkAttribute : PostNodeAttributeBase
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase BoardLink { get; set; }
    }
}