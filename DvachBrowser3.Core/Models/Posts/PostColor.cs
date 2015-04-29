using System.Runtime.Serialization;
using Windows.UI;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Цвет.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostColor
    {
        /// <summary>
        /// Красный.
        /// </summary>
        [DataMember]
        public byte R { get; set; }

        /// <summary>
        /// Зелёный.
        /// </summary>
        [DataMember]
        public byte G { get; set; }

        /// <summary>
        /// Синий.
        /// </summary>
        [DataMember]
        public byte B { get; set; }

        /// <summary>
        /// Цвет.
        /// </summary>
        [IgnoreDataMember]
        public Color Value
        {
            get
            {
                return Color.FromArgb(255, R, G, B);
            }
        }
         
    }
}