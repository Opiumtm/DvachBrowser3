using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс, поддерживающий оповещение об изменении свойств.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Свойство изменилось.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызов события по изменению свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected void TriggerPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Обратный вызов по изменению свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {            
        }
    }
}