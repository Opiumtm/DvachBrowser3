using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Дерево поста.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostTree
    {
        /// <summary>
        /// Ссылка на пост.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase ParentLink { get; set; }

        /// <summary>
        /// Текст.
        /// </summary>
        [DataMember]
        public PostNodes Comment { get; set; }

        /// <summary>
        /// Цитаты этого поста.
        /// </summary>
        [DataMember]
        public List<BoardLinkBase> Quotes { get; set; }

        /// <summary>
        /// Медиа.
        /// </summary>
        [DataMember]
        public List<PostMediaBase> Media { get; set; }

        /// <summary>
        /// Заголовок поста.
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Дата в посте.
        /// </summary>
        [DataMember]
        public string BoardSpecificDate { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Флаги.
        /// </summary>
        [DataMember]
        public PostFlags Flags { get; set; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        [DataMember]
        public int Counter { get; set; }

        /// <summary>
        /// Хэш поста.
        /// </summary>
        [DataMember]
        public string Hash { get; set; }

        /// <summary>
        /// Адрес почты.
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Расширения.
        /// </summary>
        [DataMember]
        public List<PostTreeExtension> Extensions { get; set; }
    }
}