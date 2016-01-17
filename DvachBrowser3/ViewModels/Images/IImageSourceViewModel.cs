using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Источник изображения.
    /// </summary>
    public interface IImageSourceViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Изображение.
        /// </summary>
        ImageSource Image { get; }

        /// <summary>
        /// Изображение загружено.
        /// </summary>
        bool ImageLoaded { get; }

        /// <summary>
        /// Операция загрузки.
        /// </summary>
        IOperationViewModel Load { get; }

        /// <summary>
        /// Устанавливать значение изображения.
        /// </summary>
        bool SetImageSource { get; set; }

        /// <summary>
        /// Изображение получено.
        /// </summary>
        event ImageSourceGotEventHandler ImageSourceGot;
    }
}