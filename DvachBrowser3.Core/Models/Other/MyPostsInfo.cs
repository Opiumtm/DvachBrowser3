using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Other
{
    /// <summary>
    /// Информация о собственных постах.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class MyPostsInfo
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Мои посты.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> MyPosts { get; set; }
    }
}