using System.ComponentModel;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Часть поста.
    /// </summary>
    public abstract class PostPartViewModelBase : ViewModelBase, IPostPartViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        protected PostPartViewModelBase(IPostViewModel parent)
        {
            Parent = parent;
        }

        public IPostViewModel Parent { get; }
    }
}