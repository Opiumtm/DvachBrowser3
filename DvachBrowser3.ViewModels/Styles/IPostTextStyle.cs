using Windows.UI.Xaml.Media;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Стиль текста.
    /// </summary>
    public interface IPostTextStyle
    {
        /// <summary>
        /// Квота.
        /// </summary>
        Brush Quote { get; }

        /// <summary>
        /// Обычный текст.
        /// </summary>
        Brush NormalText { get; }
    }
}