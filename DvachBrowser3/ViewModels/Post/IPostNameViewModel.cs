using System.ComponentModel;
using Windows.UI;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Имя в посте.
    /// </summary>
    public interface IPostNameViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительcкая модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Трипкод.
        /// </summary>
        string TripCode { get; }

        /// <summary>
        /// Иконка.
        /// </summary>
        IImageSourceViewModel Icon { get; }

        /// <summary>
        /// Имя иконки.
        /// </summary>
        string IconName { get; }

        /// <summary>
        /// Флаг.
        /// </summary>
        IImageSourceViewModel Flag { get; }
    }
}