using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Обёртка для фабрики тасков.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public sealed class ViewModelTaskFactoryOperationWrapper<T> : ViewModelOperationBase<T>
    {
        private readonly Func<CancellationToken, Task<T>> _taskFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="taskFactory"></param>
        public ViewModelTaskFactoryOperationWrapper(CoreDispatcher dispatcher,
            Func<CancellationToken, Task<T>> taskFactory) : base(dispatcher)
        {
            if (taskFactory == null) throw new ArgumentNullException(nameof(taskFactory));
            this._taskFactory = taskFactory;
        }

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public override async Task<T> StartTracking2()
        {
            try
            {
                if (CurrentState != ViewModelOperationState.Uninitialized && CurrentState != ViewModelOperationState.Cancelled)
                {
                    throw new InvalidOperationException("Задача уже была запущена");
                }
                if (CurrentState == ViewModelOperationState.Cancelled)
                {
                    throw new OperationCanceledException();
                }
                using (var ts = new CancellationTokenSource())
                {
                    await SetCancelAction(() =>
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        ts.Cancel();
                        return Task.CompletedTask;
                    });
                    T result = default(T);
                    try
                    {
                        await UpdateProgress(ViewModelOperationState.Active);
                        var token = ts.Token;
                        token.ThrowIfCancellationRequested();
                        var task = _taskFactory(token);
                        if (task != null)
                        {
                            result = await task;
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
                if (CurrentState != ViewModelOperationState.Cancelled)
                {
                    await UpdateProgress(ViewModelOperationState.Cancelled);
                }
                throw;
            }
            catch (Exception ex)
            {
                await UpdateProgress(ex);
                throw;
            }
        }
    }
}