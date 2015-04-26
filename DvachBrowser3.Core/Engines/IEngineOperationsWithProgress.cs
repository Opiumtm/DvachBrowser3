using System;
using System.Threading;
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
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        Task<TResult> Complete(CancellationToken token);

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        event EventHandler<TProgress> Progress;
    }
}