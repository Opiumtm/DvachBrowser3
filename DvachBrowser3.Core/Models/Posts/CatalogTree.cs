using System.Runtime.Serialization;
using DvachBrowser3.Engines;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Каталог.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class CatalogTree : PostTreeCollection
    {
        /// <summary>
        /// Штамп изменения.
        /// </summary>
        [DataMember]
        public string ETag { get; set; }

        /// <summary>
        /// Режим сортировки.
        /// </summary>
        [DataMember]
        public CatalogSortMode SortMode { get; set; }
    }
}