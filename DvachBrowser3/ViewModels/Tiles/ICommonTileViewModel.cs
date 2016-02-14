using System.ComponentModel;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Links;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тайл интерфейса.
    /// </summary>
    public interface ICommonTileViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Борда.
        /// </summary>
        string Board { get; }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        IImageSourceViewModel Image { get; }

        /// <summary>
        /// Есть новые посты.
        /// </summary>
        bool HasNewPosts { get; }

        /// <summary>
        /// Новые посты.
        /// </summary>
        int NewPosts { get; }

        /// <summary>
        /// Логотип ресурса.
        /// </summary>
        ImageSource ResourceLogo { get; }

        /// <summary>
        /// Движок.
        /// </summary>
        string Engine { get; }

        /// <summary>
        /// Ресурс.
        /// </summary>
        string Resource { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        IStyleManager StyleManager { get; }

        /// <summary>
        /// Номер треда.
        /// </summary>
        int ThreadNumber { get; }
    }
}