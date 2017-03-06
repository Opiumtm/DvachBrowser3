using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Базовый класс объекта с поддержкой диспетчеризации.
    /// </summary>
    public abstract class DispatchedObjectBase : INotifyPropertyChanged
    {
        protected CoreDispatcher Dispatcher { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        protected DispatchedObjectBase(CoreDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Состояние свойства изменилось (автоматическое получение имени свойства).
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChangedAuto([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Состояние свойства изменилось.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Проверить, производится ли доступ из UI-потока.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CheckThreadAccess()
        {
            if (Dispatcher != null)
            {
                if (!Dispatcher.HasThreadAccess)
                {
                    throw new InvalidOperationException("Доступ к члену класса не из UI-потока");
                }
            }
        }

        /// <summary>
        /// Получить значение, предварительно проверив доступ из UI-потока.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="getValue">Функция получения значения.</param>
        /// <returns>Результат.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T CheckThreadAccess<T>(Func<T> getValue)
        {
            if (getValue == null) throw new ArgumentNullException(nameof(getValue));
            CheckThreadAccess();
            return getValue();
        }
    }
}