using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Makaba
{
    /// <summary>
    /// Данные макабы.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class MakabaEntityTree
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// Имя борды.
        /// </summary>
        [DataMember]
        public string BoardName { get; set; }

        /// <summary>
        /// Баннер.
        /// </summary>
        [DataMember]
        public string BoardBannerImage { get; set; }

        /// <summary>
        /// Ссылка на борду (короткое имя).
        /// </summary>
        [DataMember]
        public string BoardBannerLink { get; set; }

        /// <summary>
        /// Скорость борды.
        /// </summary>
        [DataMember]
        public string BoardSpeed { get; set; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        [DataMember]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Текущий тред.
        /// </summary>
        [DataMember]
        public int CurrentThread { get; set; }

        /// <summary>
        /// Иконки.
        /// </summary>
        [DataMember]
        public List<MakabaIconReference> Icons { get; set; }

        /// <summary>
        /// Страницы.
        /// </summary>
        [DataMember]
        public List<int> Pages { get; set; }

        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public bool IsBoard { get; set; }

        /// <summary>
        /// Является индексом.
        /// </summary>
        [DataMember]
        public bool IsIndex { get; set; }

        /// <summary>
        /// Тэги тредов.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> ThreadTags { get; set; }
    }
}