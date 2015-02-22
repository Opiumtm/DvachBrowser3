using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Дерево постов в ветке.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadTree : PostTreeCollection
    {
        /// <summary>
        /// Время модификации.
        /// </summary>
        [DataMember]
        public string LastModified { get; set; }

        /// <summary>
        /// Получить посты (только для режима Internal).
        /// </summary>
        /// <returns>Посты.</returns>
        public override List<PostTree> GetInternalPosts()
        {
            return null;
        }

        /// <summary>
        /// Получить режим.
        /// </summary>
        /// <returns>Режим.</returns>
        protected override PostTreeCollectionMode GetMode()
        {
            return PostTreeCollectionMode.ExternalFiles;
        }
    }
}