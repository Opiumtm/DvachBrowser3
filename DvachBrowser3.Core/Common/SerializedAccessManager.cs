using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Менеджер последовательного доступа к ресурсу.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public sealed class SerializedAccessManager<T> : IConcurrenctyDispatcher<T>
    {
        private readonly ConcurrentQueue<ActionData> actions = new ConcurrentQueue<ActionData>();

        private readonly AutoResetEvent taskEvent = new AutoResetEvent(false);

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SerializedAccessManager()
        {
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
                        try
                        {
                            var r = await data.Action();
                            data.CompletionSource.SetResult(r);
                        }
                        catch (Exception ex)
                        {
                            data.CompletionSource.SetException(ex);
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
                CompletionSource = tcs
            });
            taskEvent.Set();
            return await tcs.Task;
        }

        private struct ActionData
        {
            public Func<Task<T>> Action;
            public TaskCompletionSource<T> CompletionSource;
        }

        /// <summary>
        /// Выполняющаяся задача.
        /// </summary>
        public Task SerializedTask { get; }
    }
}