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
        /// 
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
    }
}