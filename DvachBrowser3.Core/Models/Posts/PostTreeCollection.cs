using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(ThreadTree))]
    [KnownType(typeof(ThreadPreviewTree))]
    public abstract class PostTreeCollection : IPostTreeCollection
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
        /// Количество постов.
        /// </summary>
        [DataMember]
        public int PostCount { get; set; }

        /// <summary>
        /// Расширения.
        /// </summary>
        [DataMember]
        public List<PostTreeCollectionExtension> Extensions { get; set; }

        /// <summary>
        /// Режим.
        /// </summary>
        [IgnoreDataMember]
        public PostTreeCollectionMode CollectionMode
        {
            get { return GetMode(); }
        }

        /// <summary>
        /// Получить посты (только для режима Internal).
        /// </summary>
        /// <returns>Посты.</returns>
        public abstract List<PostTree> GetInternalPosts();

        /// <summary>
        /// Получить режим.
        /// </summary>
        /// <returns>Режим.</returns>
        protected abstract PostTreeCollectionMode GetMode();
    }
}