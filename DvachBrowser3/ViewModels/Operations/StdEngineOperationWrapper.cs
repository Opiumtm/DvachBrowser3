using System;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Обёртка над стандартной операцией движка.
    /// </summary>
    /// <typeparam name="TResult">Результат.</typeparam>
    public sealed class StdEngineOperationWrapper<TResult> : EngineOperationWrapper<TResult, EngineProgress>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="operationFactory">Фабрика операций.</param>
        public StdEngineOperationWrapper(Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory) : base(operationFactory)
        {
        }

        /// <summary>
        /// Получить прогресс.
        /// </summary>
        /// <param name="progress">Объект прогресса.</param>
        /// <returns>Прогресс.</returns>
        protected override double? GetProgress(EngineProgress progress)
        {
            return progress.Percent;
        }

        /// <summary>
        /// Получить сообщение.
        /// </summary>
        /// <param name="progress">Объект прогресса.</param>
        /// <returns>Сообщение.</returns>
        protected override string GetMessage(EngineProgress progress)
        {
            return progress.Message;
        }
    }
}