using System;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Ссылка на пост на борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeBoardLink : PostNodeBase
    {
        /// <summary>
        /// Ссылка на борду.
        /// </summary>
        [DataMember]
        public BoardLinkBase BoardLink { get; set; }
    }
}