using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// ������ �� �����.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeBoardLinkAttribute : PostNodeAttributeBase
    {
        /// <summary>
        /// ������.
        /// </summary>
        [DataMember]
        public BoardLinkBase BoardLink { get; set; }
    }
}