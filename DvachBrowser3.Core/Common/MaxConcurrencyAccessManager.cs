using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;

namespace DvachBrowser3
{
    /// <summary>
    /// Ограничитель максимального количества параллельных выполнений.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public sealed class MaxConcurrencyAccessManager<T> : IConcurrenctyDispatcher<T>
    {
        private readonly ConcurrentQueue<ActionData> actions = new ConcurrentQueue<ActionData>();

        private readonly AutoResetEvent taskEvent = new AutoResetEvent(false);

        private readonly ConcurrentDictionary<Guid, object> currentActions = new ConcurrentDictionary<Guid, object>();

        private readonly int maxConcurrency;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MaxConcurrencyAccessManager(int maxConcurrency)
        {
            this.maxConcurrency = maxConcurrency;
            SerializedTask = Task.Factory.StartNew(SerializedTaskProc, TaskCreationOptions.LongRunning);
        }

        private async void SerializedTaskProc()
        {
            for (;;)
            {
                try
                {
                    taskEvent.WaitOne(TimeSpan.FromSeconds(15));
                    ActionData data;
                    while (actions.TryDequeue(out data))
                    {
                        DoRun(data);
                        if (currentActions.Count >= maxConcurrency)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }

        private void DoRun(ActionData data)
        {
            currentActions.TryAdd(data.Id, new object());
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var r = await data.Action();
                    data.CompletionSource.SetResult(r);
                }
                catch (Exception ex)
                {
                    data.CompletionSource.SetException(ex);
                }
                finally
                {
                    object o;
                    currentActions.TryRemove(data.Id, out o);
                    taskEvent.Set();
                }
            });
        }

        /// <summary>
        /// Запланировать действие.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public async Task<T> QueueAction(Func<Task<T>> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var tcs = new TaskCompletionSource<T>();
            actions.Enqueue(new ActionData()
            {
                Action = action,
                CompletionSource = tcs,
                Id = Guid.NewGuid()
            });
            taskEvent.Set();
            return await tcs.Task;
        }

        private struct ActionData
        {
            public Func<Task<T>> Action;
            public TaskCompletionSource<T> CompletionSource;
            public Guid Id;
        }

        /// <summary>
        /// Выполняющаяся задача.
        /// </summary>
        public Task SerializedTask { get; }
    }
}