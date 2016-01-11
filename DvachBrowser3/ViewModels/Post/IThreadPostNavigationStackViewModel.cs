using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Стек навигации по постам в треде.
    /// </summary>
    public interface IThreadPostNavigationStackViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IThreadViewModel Parent { get; }

        /// <summary>
        /// Длина стека.
        /// </summary>
        int StackLength { get; }
    }
}