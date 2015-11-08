using System.ComponentModel;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public interface IPostViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        IPostCollectionViewModel Parent { get; }

        /// <summary>
        /// Текст поста.
        /// </summary>
        IPostTextViewModel Text { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Subject { get; }
    }
}