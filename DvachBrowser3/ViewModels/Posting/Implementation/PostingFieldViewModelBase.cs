using System.Collections.Generic;
using DvachBrowser3.Posting;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовый класс поля постинга.
    /// </summary>
    /// <typeparam name="T">Тип поля.</typeparam>
    public abstract class PostingFieldViewModelBase<T> : ViewModelBase, IPostingFieldViewModel<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="isSupported">Поддерживается.</param>
        /// <param name="role">Роль.</param>
        protected PostingFieldViewModelBase(IPostingFieldsViewModel parent, bool isSupported, PostingFieldSemanticRole role)
        {
            Parent = parent;
            IsSupported = isSupported;
            Role = role;
        }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostingFieldsViewModel Parent { get; }

        /// <summary>
        /// Как базовый интерфейс.
        /// </summary>
        public IPostingFieldViewModel<T> AsBaseIntf => this;

        /// <summary>
        /// Поддерживается.
        /// </summary>
        public bool IsSupported { get; }

        /// <summary>
        /// Роль.
        /// </summary>
        public PostingFieldSemanticRole Role { get; }

        private T objValue;

        /// <summary>
        /// Значение.
        /// </summary>
        public T Value
        {
            get { return objValue; }
            set
            {
                objValue = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Получить данные постинга.
        /// </summary>
        /// <returns>Данные постинга.</returns>
        public virtual KeyValuePair<PostingFieldSemanticRole, object>? GetValueData()
        {
            return new KeyValuePair<PostingFieldSemanticRole, object>(Role, Value);
        }

        /// <summary>
        /// Заполнить значение.
        /// </summary>
        /// <param name="data">Значение.</param>
        public virtual void SetValueData(object data)
        {
            if (data == null)
            {
                Value = default(T);
            }
            else
            {
                Value = (T)data;
            }
        }
    }
}