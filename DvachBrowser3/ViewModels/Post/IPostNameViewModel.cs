using System.ComponentModel;
using Windows.UI;

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
    }
}