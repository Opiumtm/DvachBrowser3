using System;
using System.ComponentModel;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Часть поста.
    /// </summary>
    public abstract class PostPartViewModelBase : ViewModelBase
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

        private readonly Lazy<IStyleManager> styleManager = new Lazy<IStyleManager>(() => StyleManagerFactory.Current.GetManager());

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => styleManager.Value;
    }
}