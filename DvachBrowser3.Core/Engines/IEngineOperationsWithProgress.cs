using System;
using System.Threading.Tasks;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Операция движка с прогрессом.
    /// </summary>
    /// <typeparam name="TResult">Результат.</typeparam>
    /// <typeparam name="TProgress">Прогресс.</typeparam>
    public interface IEngineOperationsWithProgress<TResult, TProgress> where TProgress : EventArgs
    {
        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <returns>Таск.</returns>
        Task<TResult> Complete();

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        event EventHandler<TProgress> Progress;

        /// <summary>
        /// Отменить.
        /// </summary>
        void Cancel();
    }
}