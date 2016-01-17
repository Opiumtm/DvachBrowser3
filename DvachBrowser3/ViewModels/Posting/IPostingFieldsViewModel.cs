using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поля постинга.
    /// </summary>
    public interface IPostingFieldsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostingViewModel Parent { get; } 
    }
}