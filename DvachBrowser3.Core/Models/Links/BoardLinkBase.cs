using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Базовый класс для ссылки на борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(BoardLink))]
    [KnownType(typeof(BoardPageLink))]
    [KnownType(typeof(ThreadLink))]
    [KnownType(typeof(PostLink))]
    [KnownType(typeof(BoardMediaLink))]
    [KnownType(typeof(MediaLink))]
    [KnownType(typeof(YoutubeLink))]
    [KnownType(typeof(RootLink))]
    public abstract class BoardLinkBase
    {
        /// <summary>
        /// Движок.
        /// </summary>
        [DataMember]
        public string Engine { get; set; }
   }
}