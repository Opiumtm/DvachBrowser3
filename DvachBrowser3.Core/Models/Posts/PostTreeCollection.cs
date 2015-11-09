using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Other;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(ThreadTree))]
    [KnownType(typeof(ThreadPreviewTree))]
    [KnownType(typeof(ThreadTreePartial))]
    [KnownType(typeof(ArchiveThreadTree))]
    public abstract class PostTreeCollection : IPostTreeListSource
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase ParentLink { get; set; }

        /// <summary>
        /// Посты.
        /// </summary>
        [DataMember]
        public List<PostTree> Posts { get; set; }


        /// <summary>
        /// Расширения.
        /// </summary>
        [DataMember]
        public List<PostTreeCollectionExtension> Extensions { get; set; }

        /// <summary>
        /// Получить посты.
        /// </summary>
        /// <returns>Посты.</returns>
        Task<IList<PostTree>> IPostTreeListSource.GetPosts()
        {
            return Task.FromResult<IList<PostTree>>(Posts);
        }
    }
}