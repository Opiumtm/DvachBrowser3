using System.ComponentModel;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель треда.
    /// </summary>
    public interface IThreadTileViewModel : INotifyPropertyChanged
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
        /// Номер треда.
        /// </summary>
        int ThreadNumber { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

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
    }
}