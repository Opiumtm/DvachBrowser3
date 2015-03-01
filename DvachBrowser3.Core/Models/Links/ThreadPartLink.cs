using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на часть треда.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadPartLink : ThreadLink
    {
        /// <summary>
        /// Начиная с поста.
        /// </summary>
        [DataMember]
        public int FromPost { get; set; }

        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected override BoardLinkKind GetLinkKind()
        {
            return BoardLinkKind.Thread | BoardLinkKind.PartialThread;
        }
    }
}