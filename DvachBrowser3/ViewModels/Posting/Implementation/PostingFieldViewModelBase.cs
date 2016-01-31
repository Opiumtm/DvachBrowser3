using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // ReSharper disable once InconsistentNaming
        protected T objValue;

        /// <summary>
        /// Значение.
        /// </summary>
        public T Value
        {
            get { return objValue; }
            set
            {
                if (!IsEquals(objValue, value))
                {
                    objValue = value;
                    RaisePropertyChanged();
                    OnValueChange();
                }
            }
        }

        /// <summary>
        /// Обновить значение без вызова NotifyPropertyChanged.
        /// </summary>
        /// <param name="value">Значение.</param>
        public void UpdateValueInternal(T value)
        {
            objValue = value;
            OnValueChange();
        }

        /// <summary>
        /// Вызвать событие по изменению.
        /// </summary>
        public void RaiseChanged()
        {
            RaisePropertyChanged();
        }

        /// <summary>
        /// Проверка на равенство значений.
        /// </summary>
        /// <param name="oldValue">Старое значение.</param>
        /// <param name="newValue">Новое значение.</param>
        /// <returns>Результат.</returns>
        protected virtual bool IsEquals(T oldValue, T newValue)
        {
            return EqualityComparer<T>.Default.Equals(oldValue, newValue);
        }

        /// <summary>
        /// Изменение значения.
        /// </summary>
        protected virtual void OnValueChange()
        {
            if (Parent != null)
            {
                AppHelpers.ActionOnUiThread(async () =>
                {
                    await Parent.Flush(false);
                });
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
        /// <param name="flush">Вызвать сохранение данных.</param>
        public virtual void SetValueData(object data, bool flush = true)
        {
            if (data?.GetType() == typeof(T))
            {
                objValue = (T)data;
            }
            else
            {
                objValue = DefaultValue;
            }
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(nameof(Value));
            if (flush)
            {
                OnValueChange();
            }
        }

        /// <summary>
        /// Заполнить значение по умолчанию.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public virtual void SetDefaultValueData(bool flush = true)
        {
            objValue = DefaultValue;
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(nameof(Value));
            if (flush)
            {
                OnValueChange();
            }
        }

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public virtual Task Clear(bool flush = true)
        {
            objValue = DefaultValue;
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(nameof(Value));
            if (flush)
            {
                OnValueChange();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        public T DefaultValue { get; set; }
    }
}