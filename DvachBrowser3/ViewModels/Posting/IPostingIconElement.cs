using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public interface IPostingIconElement : INotifyPropertyChanged
    {
        /// <summary>
        /// Значение.
        /// </summary>
        int? Value { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        IImageSourceViewModel Image { get; }
    }
}