using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DvachBrowser3
{
    /// <summary>
    /// Обозреваемый объект.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызвать событие.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            PropertyChangedCallback(propertyName);
        }

        /// <summary>
        /// Обратный вызов по изменению свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void PropertyChangedCallback(string propertyName)
        {
        }
    }
}