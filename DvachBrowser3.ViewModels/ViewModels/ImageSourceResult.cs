using System;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Результат с изображением.
    /// </summary>
    public sealed class ImageSourceResult : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="image">Изображение.</param>
        public ImageSourceResult(ImageSource image)
        {
            Image = image;
        }

        /// <summary>
        /// Изображение.
        /// </summary>
        public ImageSource Image { get; private set; }
    }
}