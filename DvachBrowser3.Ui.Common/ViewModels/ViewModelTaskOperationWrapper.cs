using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Обёртка для таска.
    /// </summary>
    public sealed class ViewModelTaskOperationWrapper : ViewModelOperationBase
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly Task _wrappedTask;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="wrappedTask">Таск.</param>
        /// <param name="tokenSource">Источник токенов отмены.</param>
        public ViewModelTaskOperationWrapper(int id, CoreDispatcher dispatcher, Task wrappedTask, CancellationTokenSource tokenSource = null) : base(id, dispatcher)
        {
            if (wrappedTask == null) throw new ArgumentNullException(nameof(wrappedTask));
            this._tokenSource = tokenSource;
            this._wrappedTask = wrappedTask;
        }

        private async Task RunTask(Task task, CancellationTokenSource tokenSource)
        {
            try
            {
                var token = tokenSource?.Token ?? CancellationToken.None;
                token.ThrowIfCancellationRequested();
                if (tokenSource != null)
                {
                    await SetCancelAction(() =>
                    {
                        tokenSource.Cancel();
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    await SetCancelAction(null);
                }
                try
                {
                    await UpdateProgress(ViewModelOperationState.Active);
                    await task;
                }
                finally
                {
                    if (tokenSource != null)
                    {
                        await SetCancelAction(null);
                    }
                }
                token.ThrowIfCancellationRequested();
                await UpdateProgress(ViewModelOperationState.Finished);
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

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public override async Task StartTracking()
        {
            await RunTask(_wrappedTask, _tokenSource);
        }
    }
}