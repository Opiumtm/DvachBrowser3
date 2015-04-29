using System;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Other
{
    /// <summary>
    /// Информация о количестве постов.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostCountInfo
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Количество постов.
        /// </summary>
        [DataMember]
        public int PostCount { get; set; }

        /// <summary>
        /// Количество загруженных постов.
        /// </summary>
        [DataMember]
        public int LoadedPostCount { get; set; }

        /// <summary>
        /// Последнее изменение.
        /// </summary>
        [DataMember]
        public DateTime LastChange { get; set; }

        /// <summary>
        /// Последний просмотр.
        /// </summary>
        [DataMember]
        public DateTime LastView { get; set; }

        /// <summary>
        /// Тред скрыт.
        /// </summary>
        [DataMember]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Результат.</returns>
        public PostCountInfo Clone()
        {
            return new PostCountInfo()
            {
                IsHidden = IsHidden,
                LastChange = LastChange,
                LastView = LastView,
                Link = Link,
                LoadedPostCount = LoadedPostCount,
                PostCount = PostCount
            };
        }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public static PostCountInfo Create(BoardLinkBase link)
        {
            return new PostCountInfo()
            {
                IsHidden = false,
                LastChange = DateTime.MinValue,
                LastView = DateTime.MinValue,
                Link = link,
                LoadedPostCount = 0,
                PostCount = 0
            };
        }
    }
}