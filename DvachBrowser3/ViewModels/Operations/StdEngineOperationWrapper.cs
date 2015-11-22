using System;
using System.Dynamic;
using DvachBrowser3.Engines;
using Microsoft.CSharp.RuntimeBinder;

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
        public StdEngineOperationWrapper(Func<object, IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory) : base(operationFactory)
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

        /// <summary>
        /// Получить статус ожидания.
        /// </summary>
        /// <param name="progress">Прогресс.</param>
        /// <returns>Статус ожидания.</returns>
        protected override bool GetIsWaiting(EngineProgress progress)
        {
            try
            {
                return progress.OtherData.Kind == "WAIT";
            }
            catch
            {
                return false;
            }
        }
    }
}