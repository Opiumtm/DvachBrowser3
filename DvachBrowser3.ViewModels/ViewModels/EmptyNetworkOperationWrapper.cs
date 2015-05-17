using System;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Пустая операция.
    /// </summary>
    public sealed class EmptyNetworkOperationWrapper : ViewModelBase, INetworkViewModel
    {
        /// <summary>
        /// Можно выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Результат.</returns>
        public bool CanExecute(object parameter)
        {
            return false;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public void Execute(object parameter)
        {
        }

        /// <summary>
        /// Возможность выполнения операции изменилась.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        public void ExecuteOperation()
        {
        }

        /// <summary>
        /// Операция выполняется.
        /// </summary>
        public bool IsExecuting
        {
            get { return false; }
        }

        /// <summary>
        /// Есть ошибка.
        /// </summary>
        public bool IsError
        {
            get { return false; }
        }

        /// <summary>
        /// Операция завершена успешно.
        /// </summary>
        public bool IsOk
        {
            get { return true; }
        }

        /// <summary>
        /// Можно выполнять.
        /// </summary>
        public bool IsCanExecute
        {
            get { return false; }
        }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public event EventHandler Error;

        /// <summary>
        /// Завершено.
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Начато.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string ErrorText
        {
            get { return null; }
        }
    }
}