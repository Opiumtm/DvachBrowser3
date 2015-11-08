using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public interface IBoardInfoIcon : INotifyPropertyChanged
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Иконка.
        /// </summary>
        IImageSourceViewModel Icon { get; }
    }
}