using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// —сылка на борду.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeBoardLinkAttribute : PostNodeAttributeBase
    {
        /// <summary>
        /// —сылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase BoardLink { get; set; }
    }
}