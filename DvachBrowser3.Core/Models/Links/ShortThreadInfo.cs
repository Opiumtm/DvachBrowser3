using System;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Короткая информация о треде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    [KnownType(typeof(FavoriteThreadInfo))]
    public class ShortThreadInfo : IDeepCloneable<ShortThreadInfo>
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Время обновления.
        /// </summary>
        [DataMember]
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Время добавления.
        /// </summary>
        [DataMember]
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Время просмотра.
        /// </summary>
        [DataMember]
        public DateTime ViewDate { get; set; }

        /// <summary>
        /// Маленькое изображение.
        /// </summary>
        [DataMember]
        public ThreadPictureInfo SmallImage { get; set; }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public virtual ShortThreadInfo DeepClone()
        {
            return new ShortThreadInfo()
            {
                Title = Title,
                AddedDate = AddedDate,
                CreatedDate = CreatedDate,
                SmallImage = SmallImage?.DeepClone(),
                UpdatedDate = UpdatedDate,
                ViewDate = ViewDate
            };
        }
    }
}