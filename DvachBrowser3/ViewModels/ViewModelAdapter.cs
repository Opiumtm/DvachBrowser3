using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Адаптер модели представления.
    /// </summary>
    /// <typeparam name="T">Тип модели.</typeparam>
    public abstract class ViewModelAdapter<T> : INotifyPropertyChanged where T: class, INotifyPropertyChanged
    {
        private readonly PropertyChangedEventListener<T> listener = new PropertyChangedEventListener<T>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected ViewModelAdapter()
        {
            listener.PropertyChanged += ViewModelPropertyChanged;
        }

        /// <summary>
        /// Изменено значение модели.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Событие.</param>
        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewModelPropertyChanged(sender as T, e?.PropertyName);
        }

        /// <summary>
        /// Значение модели изменено.
        /// </summary>
        /// <param name="sender">Модель.</param>
        /// <param name="propertyName">Свойство.</param>
        protected virtual void ViewModelPropertyChanged(T sender, string propertyName)
        {            
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        protected T ViewModel { get; private set; }

        /// <summary>
        /// Установить модель.
        /// </summary>
        /// <param name="oldValue">Старая модель.</param>
        /// <param name="newValue">Новая модель.</param>
        public void SetViewModel(T oldValue, T newValue)
        {
            ViewModel = newValue;
            listener.SetViewModel(oldValue, newValue);
        }

        /// <summary>
        /// Установить модель.
        /// </summary>
        /// <param name="e">Событие dependency property.</param>
        public void SetViewModel(DependencyPropertyChangedEventArgs e)
        {
            SetViewModel(e.OldValue as T, e.NewValue as T);
        }

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}