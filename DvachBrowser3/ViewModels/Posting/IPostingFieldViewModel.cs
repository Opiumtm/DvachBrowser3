using System.Collections.Generic;
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
        /// Как базовый интерфейс.
        /// </summary>
        IPostingFieldViewModel<T> AsBaseIntf { get; }

        /// <summary>
        /// Поддерживается.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Значение.
        /// </summary>
        T Value { get; set; }
    }
}