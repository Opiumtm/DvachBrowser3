using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using DvachBrowser3.Engines;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Враппер операции движка.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TProgress">Прогресс операции.</typeparam>
    public abstract class ViewModelEngineOperationWrapper<T, TProgress> : ViewModelOperationBase<T> where TProgress : EventArgs
    {
        private readonly IEngineOperationsWithProgress<T, TProgress> _operation;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="operation">Операция.</param>
        protected ViewModelEngineOperationWrapper(CoreDispatcher dispatcher, IEngineOperationsWithProgress<T, TProgress> operation) : base(dispatcher)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            this._operation = operation;
        }

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public override async Task<T> StartTracking2()
        {
            try
            {
                using (var ts = new CancellationTokenSource())
                {
                    await SetCancelAction(() =>
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        ts.Cancel();
                        return Task.CompletedTask;
                    });
                    T result;
                    try
                    {
                        await UpdateProgress(ViewModelOperationState.Active);
                        var token = ts.Token;
                        _operation.Progress += OperationOnProgress;
                        try
                        {
                            var task = _operation.Complete(token);
                            result = await task;
                        }
                        finally
                        {
                            _operation.Progress -= OperationOnProgress;
                        }
                    }
                    finally
                    {
                        await SetCancelAction(null);
                    }
                    await UpdateProgress(result);
                    return result;
                }
            }
            catch (OperationCanceledException)
            {
                await UpdateProgress(ViewModelOperationState.Cancelled);
                throw;
            }
            catch (Exception ex)
            {
                await UpdateProgress(ex);
                throw;
            }
        }

        /// <summary>
        /// Действие по прогрессу.
        /// </summary>
        /// <param name="sender">Источник.</param>
        /// <param name="progress">Прогресс.</param>
        protected abstract void OperationOnProgress(object sender, TProgress progress);
    }

    /// <summary>
    /// Враппер операции движка.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public class ViewModelEngineOperationWrapper<T> : ViewModelEngineOperationWrapper<T, EngineProgress>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="operation">Операция.</param>
        public ViewModelEngineOperationWrapper(CoreDispatcher dispatcher, IEngineOperationsWithProgress<T, EngineProgress> operation) : base(dispatcher, operation)
        {
        }

        /// <summary>
        /// Действие по прогрессу.
        /// </summary>
        /// <param name="sender">Источник.</param>
        /// <param name="progress">Прогресс.</param>
        protected override async void OperationOnProgress(object sender, EngineProgress progress)
        {
            try
            {
                await UpdateProgress(ViewModelOperationState.Active, progress.Percent, progress.Message, null, progress.OtherData, null);
            }
            catch (OperationCanceledException)
            {
                // игнорируем ошибки, возникающие при отмене операции
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }
    }
}