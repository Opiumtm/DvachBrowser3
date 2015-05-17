using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Класс помощник для результата с изображением.
    /// </summary>
    public static class ImageSourceResultHelper
    {
        /// <summary>
        /// Прочитать файл как изображение.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Изображение.</returns>
        public static async Task<ImageSource> AsImageSourceAsync(this StorageFile file)
        {
            var r = new BitmapImage();
            using (var str = await file.OpenReadAsync())
            {
                await r.SetSourceAsync(str);
            }
            return r;
        }
    }
}