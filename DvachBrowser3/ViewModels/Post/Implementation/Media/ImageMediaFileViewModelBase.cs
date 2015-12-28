using System.Text;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовая модель представления изображения.
    /// </summary>
    /// <typeparam name="T">Тип медиа.</typeparam>
    public abstract class ImageMediaFileViewModelBase<T> : PostMediaFileViewModelBase<T> where T : PostImageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaData">Медиа данные.</param>
        protected ImageMediaFileViewModelBase(IPostViewModel parent, T mediaData) : base(parent, mediaData)
        {
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public override int? Height => MediaData?.Height;

        /// <summary>
        /// Ширина.
        /// </summary>
        public override int? Width => MediaData?.Width;

        /// <summary>
        /// Имя файла.
        /// </summary>
        public override string Name => MediaData?.Name;

        /// <summary>
        /// Строка с информацией.
        /// </summary>
        public override string InfoString
        {
            get
            {
                var sb = new StringBuilder();
                if (Size != null)
                {
                    var sz = Size.Value;
                    if (sz < 1024)
                    {
                        sb.Append(sz + " байт");
                    } else if (sz < 1024*1024)
                    {
                        double szs = sz/1024.0;
                        sb.AppendFormat("{0:F1} Кб", szs);
                    }
                    else
                    {
                        double szs = sz / (1024.0 * 1024.0);
                        sb.AppendFormat("{0:F1} Мб", szs);
                    }
                }
                if (Width != null && Height != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendFormat("{0}x{1}", Width.Value, Height.Value);
                }
                if (Name != null)
                {
                    var idx = Name.LastIndexOf('.');
                    if (idx >= 0)
                    {
                        var ext = Name.Substring(idx);
                        if (sb.Length > 0)
                        {
                            sb.Append(" ");
                        }
                        sb.Append(ext);
                    }
                }
                return sb.ToString();
            }
        }
    }
}