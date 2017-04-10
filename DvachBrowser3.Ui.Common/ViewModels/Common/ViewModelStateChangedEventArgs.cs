using System;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Данные события по изменению состояния модели представления.
    /// </summary>
    public class ViewModelStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="oldState">Старое состояние.</param>
        /// <param name="newState">Новое состояние.</param>
        public ViewModelStateChangedEventArgs(ViewModelState oldState, ViewModelState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        /// <summary>
        /// Старое состояние.
        /// </summary>
        public ViewModelState OldState { get; private set; }

        /// <summary>
        /// Новое состояние.
        /// </summary>
        public ViewModelState NewState { get; private set; }
    }
}