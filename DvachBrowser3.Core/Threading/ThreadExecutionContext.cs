using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;

namespace DvachBrowser3
{
    /// <summary>
    /// Контекст выполнения треда.
    /// </summary>
    public sealed class ThreadExecutionContext : IThreadExecutionContext
    {
        private readonly AutoResetEvent executeEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent disposeEvent = new ManualResetEvent(false); 

        private int isCancelled;

        private static readonly object EmptyResult = new object();

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ThreadExecutionContext()
        {            
            contextTask = ThreadPool.RunAsync(DispatcherFunc);
        }

        private void DispatcherFunc(IAsyncAction a)
        {
            for (;;)
            {
                try
                {
                    var ev = new WaitHandle[] {executeEvent, disposeEvent};
                    WaitHandle.WaitAny(ev);
                    if (Interlocked.CompareExchange(ref isCancelled, 0, 0) != 0)
                    {
                        return;
                    }
                    ExecutionTask task;
                    while (tasks.TryDequeue(out task))
                    {
                        var tcs = task.CompletionSource;
                        try
                        {
                            var r = task.ExecFunc();
                            tcs.TrySetResult(r);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }
                        if (Interlocked.CompareExchange(ref isCancelled, 0, 0) != 0)
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                    return;
                }
            }
        }

        private readonly IAsyncAction contextTask;

        private readonly ConcurrentQueue<ExecutionTask> tasks = new ConcurrentQueue<ExecutionTask>();

        private struct ExecutionTask
        {
            public TaskCompletionSource<object> CompletionSource;
            public Func<object> ExecFunc;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public async void Dispose()
        {
            Interlocked.Exchange(ref isCancelled, 1);
            disposeEvent.Set();
            await contextTask;
            executeEvent.Dispose();
            disposeEvent.Dispose();
            ExecutionTask task;
            while (tasks.TryDequeue(out task))
            {
                task.CompletionSource.TrySetException(new ObjectDisposedException(typeof(ThreadExecutionContext).FullName));
            }
        }

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public async Task Execute(Action action)
        {
            var task = new ExecutionTask()
            {
                CompletionSource = new TaskCompletionSource<object>(),
                ExecFunc = () =>
                {
                    action();
                    return EmptyResult;
                }
            };
            var wt = task.CompletionSource.Task;
            tasks.Enqueue(task);
            executeEvent.Set();
            await wt;
        }

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="func"></param>
        /// <returns>Результат.</returns>
        public async Task<T> Execute<T>(Func<T> func)
        {
            var task = new ExecutionTask()
            {
                CompletionSource = new TaskCompletionSource<object>(),
                ExecFunc = () => func()
            };
            var wt = task.CompletionSource.Task;
            tasks.Enqueue(task);
            executeEvent.Set();
            var r = await wt;
            return (T) r;
        }

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <param name="action">Действие.</param>
        /// <param name="param">Параметр.</param>
        /// <returns>Таск.</returns>
        public async Task Execute<TParam>(Action<TParam> action, TParam param)
        {
            var task = new ExecutionTask()
            {
                CompletionSource = new TaskCompletionSource<object>(),
                ExecFunc = () =>
                {
                    action(param);
                    return EmptyResult;
                }
            };
            var wt = task.CompletionSource.Task;
            tasks.Enqueue(task);
            executeEvent.Set();
            await wt;
        }

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="func">Функция.</param>
        /// <param name="param">Параметр.</param>
        /// <returns>Результат.</returns>
        public async Task<T> Execute<TParam, T>(Func<TParam, T> func, TParam param)
        {
            var task = new ExecutionTask()
            {
                CompletionSource = new TaskCompletionSource<object>(),
                ExecFunc = () => func(param)
            };
            var wt = task.CompletionSource.Task;
            tasks.Enqueue(task);
            executeEvent.Set();
            var r = await wt;
            return (T) r;
        }
    }
}