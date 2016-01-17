using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Изображение постинга.
    /// </summary>
    public interface IPostingMediaViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostingMediaCollectionViewModel Parent { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        ImageSource Image { get; }

        /// <summary>
        /// Есть изображение.
        /// </summary>
        bool HasImage { get; }

        /// <summary>
        /// Строка с размером.
        /// </summary>
        string SizeStr { get; }

        /// <summary>
        /// Размер файла.
        /// </summary>
        ulong FileSize { get; }
    }
}