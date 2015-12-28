using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace DvachBrowser3
{
    /// <summary>
    /// Слушатель события по изменению свойства.
    /// </summary>
    /// <typeparam name="T">Тип модели представления.</typeparam>
    public sealed class PropertyChangedEventListener<T> : INotifyPropertyChanged where T: class , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetViewModel(T oldValue, T newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ViewModelPropertyChanged;
            }
            if (newValue != null)
            {
                newValue.PropertyChanged += ViewModelPropertyChanged;
            }
        }

        public void SetViewModel(DependencyPropertyChangedEventArgs e)
        {
            SetViewModel(e.OldValue as T, e.NewValue as T);
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e?.PropertyName);
        }        
    }
}