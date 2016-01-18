using System.Collections.Generic;
using System.ComponentModel;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поле постинга.
    /// </summary>
    /// <typeparam name="T">Тип поля.</typeparam>
    public interface IPostingFieldViewModel<T> : INotifyPropertyChanged
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
        /// Роль.
        /// </summary>
        PostingFieldSemanticRole Role { get; }

        /// <summary>
        /// Значение.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Получить данные постинга.
        /// </summary>
        /// <returns>Данные постинга.</returns>
        KeyValuePair<PostingFieldSemanticRole, object>? GetValueData();

        /// <summary>
        /// Заполнить значение.
        /// </summary>
        /// <param name="data">Значение.</param>
        void SetValueData(object data);
    }
}