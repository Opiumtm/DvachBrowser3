using Windows.UI.Xaml.Media;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления иконки.
    /// </summary>
    public interface IIconViewModel
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Иконка.
        /// </summary>
        ImageSource Icon { get; }

        /// <summary>
        /// Операция.
        /// </summary>
        INetworkViewModel NetworkOperation { get; }

        /// <summary>
        /// Высота.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        int Width { get; }
    }
}