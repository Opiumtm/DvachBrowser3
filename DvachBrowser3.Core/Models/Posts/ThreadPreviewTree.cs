using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Дерево предпросмотра постов в ветке (с общей страницы борды).
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadPreviewTree : PostTreeCollection
    {
        /// <summary>
        /// Количество изображений.
        /// </summary>
        [DataMember]
        public int ImageCount { get; set; }

        /// <summary>
        /// Пропущено изображений.
        /// </summary>
        [DataMember]
        public int OmitImages { get; set; }

        /// <summary>
        /// Пропущено постов.
        /// </summary>
        [DataMember]
        public int Omit { get; set; }

        /// <summary>
        /// Количество ответов.
        /// </summary>
        [DataMember]
        public int ReplyCount { get; set; }

        /// <summary>
        /// Первый пост.
        /// </summary>
        [IgnoreDataMember]
        public PostTree OpPost
        {
            get { return Posts != null ? Posts.FirstOrDefault() : null; }
        }
    }
}