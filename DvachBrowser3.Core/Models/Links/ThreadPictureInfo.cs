using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Информация об изображении.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class ThreadPictureInfo : IDeepCloneable<ThreadPictureInfo>
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        [DataMember]
        public BoardLinkBase Link { get; set; }

        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// Ширина.
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public ThreadPictureInfo DeepClone()
        {
            return new ThreadPictureInfo()
            {
                Link = Link.DeepClone(),
                Width = Width,
                Height = Height
            };
        }
    }
}