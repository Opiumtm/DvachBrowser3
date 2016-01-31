using System.ComponentModel;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поле постинга.
    /// </summary>
    /// <typeparam name="T">Тип поля.</typeparam>
    public interface IPostingFieldViewModel<T> : INotifyPropertyChanged, IPostingFieldDataProvider
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostingFieldsViewModel Parent { get; }

        /// <summary>
        /// Поддерживается.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Значение.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Обновить значение без вызова NotifyPropertyChanged.
        /// </summary>
        /// <param name="value">Значение.</param>
        void UpdateValueInternal(T value);
    }
}