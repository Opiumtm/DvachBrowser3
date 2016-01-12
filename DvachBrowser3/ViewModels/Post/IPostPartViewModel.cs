using System.ComponentModel;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Часть поста.
    /// </summary>
    public interface IPostPartViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }
    }
}