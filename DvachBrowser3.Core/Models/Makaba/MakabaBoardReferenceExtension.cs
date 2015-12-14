using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Board;

namespace DvachBrowser3.Makaba
{
    /// <summary>
    /// Расширение информации о борде для макабы.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class MakabaBoardReferenceExtension : BoardReferenceExtension
    {
        /// <summary>
        /// Иконки (null - нет иконок).
        /// </summary>
        [DataMember]
        public List<MakabaIconReference> Icons { get; set; }

        /// <summary>
        /// Бамплимит.
        /// </summary>
        [DataMember]
        public int? Bumplimit { get; set; }

        /// <summary>
        /// Имя по умолчанию.
        /// </summary>
        [DataMember]
        public string DefaultName { get; set; }

        /// <summary>
        /// Количество страниц.
        /// </summary>
        [DataMember]
        public int Pages { get; set; }

        /// <summary>
        /// Сажа.
        /// </summary>
        [DataMember]
        public bool Sage { get; set; }

        /// <summary>
        /// Трипкоды.
        /// </summary>
        [DataMember]
        public bool Tripcodes { get; set; }

        /// <summary>
        /// Максимальный комментарий.
        /// </summary>
        [DataMember]
        public int? MaxComment { get; set; }

        /// <summary>
        /// Разрешение тэгов.
        /// </summary>
        [DataMember]
        public bool EnableTags { get; set; }

        /// <summary>
        /// Разрешение лайков.
        /// </summary>
        [DataMember]
        public bool EnableLikes { get; set; }
    }
}