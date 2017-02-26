using System;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Данные события по изменению состояния операции модели представления.
    /// </summary>
    public class ViewModelOperationStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="oldState">Старое состояние.</param>
        /// <param name="newState">Новое состояние.</param>
        public ViewModelOperationStateChangedEventArgs(ViewModelOperationState oldState, ViewModelOperationState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        /// <summary>
        /// Старое состояние.
        /// </summary>
        public ViewModelOperationState OldState { get; private set; }

        /// <summary>
        /// Новое состояние.
        /// </summary>
        public ViewModelOperationState NewState { get; private set; }
    }
}