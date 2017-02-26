using System;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Параметр события по изменению прогресса операции.
    /// </summary>
    public class ViewModelOperationProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="progress">Прогресс операции.</param>
        public ViewModelOperationProgressEventArgs(ViewModelOperationProgress progress)
        {
            Progress = progress;
        }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public ViewModelOperationProgress Progress { get; private set; }
    }
}