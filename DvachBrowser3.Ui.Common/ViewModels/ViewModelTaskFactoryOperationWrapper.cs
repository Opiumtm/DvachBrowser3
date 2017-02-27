using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Обёртка для фабрики тасков.
    /// </summary>
    public sealed class ViewModelTaskFactoryOperationWrapper : ViewModelOperationBase
    {
        private readonly Func<CancellationToken, Task> _taskFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="taskFactory"></param>
        public ViewModelTaskFactoryOperationWrapper(int id, CoreDispatcher dispatcher,
            Func<CancellationToken, Task> taskFactory) : base(id, dispatcher)
        {
            if (taskFactory == null) throw new ArgumentNullException(nameof(taskFactory));
            this._taskFactory = taskFactory;
        }

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public override async Task StartTracking()
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
                    try
                    {
                        await UpdateProgress(ViewModelOperationState.Active);
                        var task = _taskFactory(ts.Token);
                        if (task != null)
                        {
                            await task;
                        }
                    }
                    finally
                    {
                        await SetCancelAction(null);
                    }
                    await UpdateProgress(ViewModelOperationState.Finished);
                }
            }
            catch (OperationCanceledException)
            {
                await UpdateProgress(ViewModelOperationState.Cancelled);
            }
            catch (Exception ex)
            {
                await UpdateProgress(ex);
            }
        }
    }
}