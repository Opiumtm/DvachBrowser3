using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using DvachBrowser3.Views;
using Template10.Common;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Базовый класс менеджера стиля.
    /// </summary>
    public abstract class StyleManagerObjectBase : INotifyPropertyChanged, IWeakEventCallback
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        protected StyleManagerObjectBase()
        {
            Shell.IsNarrowViewChanged.AddCallback(this);
            // ReSharper disable once VirtualMemberCallInContructor
            SetValues();
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == Shell.IsNarrowViewChangedId)
            {
                SetValues();
            }
        }

        /// <summary>
        /// Узкое представление.
        /// </summary>
        protected bool IsNarrowView => Shell.Instance?.IsNarrowView ?? false;

        /// <summary>
        /// Назначить значения.
        /// </summary>
        protected abstract void SetValues();

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Свойство изменилось.
        /// </summary>
        /// <param name="propertyName">Свойство.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}