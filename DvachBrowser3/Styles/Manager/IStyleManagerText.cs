using System.ComponentModel;
using Windows.ApplicationModel.Store;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Текст.
    /// </summary>
    public interface IStyleManagerText : INotifyPropertyChanged
    {
        /// <summary>
        /// Нормальный размер шрифтаю
        /// </summary>
        double NormalFontSize { get; }

        /// <summary>
        /// Маленький размер шрифта.
        /// </summary>
        double SmallFontSize { get; }

        /// <summary>
        /// Размер шрифта заголовков.
        /// </summary>
        double TitleFontSize { get; }

        /// <summary>
        /// Размер текста поста.
        /// </summary>
        double PostFontSize { get; }

        /// <summary>
        /// Количество линий в превью поста.
        /// </summary>
        int ThreadPreviewPostLines { get; }

        /// <summary>
        /// Размер хидера списка.
        /// </summary>
        double ListHeaderFontSize { get; }

        /// <summary>
        /// Размер шрифта прогресса.
        /// </summary>
        double ProgressFontSize { get; }
    }
}