using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Диспетчер асинхронных операций.
    /// </summary>
    public sealed class AsyncOperationDispatcher
    {
        private readonly int maxConcurrentOperations;

        private readonly Queue<Func<Task>> operations = new Queue<Func<Task>>();

        private readonly Queue<Func<Task>> hpOperations = new Queue<Func<Task>>();

        private readonly HashSet<Guid> runOperations = new HashSet<Guid>();

        private readonly Task dispatcherTask;

        private readonly AutoResetEvent taskEvent = new AutoResetEvent(false);

        private readonly AutoResetEvent finishedEvent = new AutoResetEvent(false);

        private readonly object taskLock = new object();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="maxConcurrentOperations">Максимальное число одновременных операций.</param>
        public AsyncOperationDispatcher(int maxConcurrentOperations = 10)
        {
            this.maxConcurrentOperations = maxConcurrentOperations;
            this.dispatcherTask = Task.Factory.StartNew(DispatcherAction, TaskCreationOptions.LongRunning);
        }

        private void DispatcherAction()
        {
            for (;;)
            {
                WaitHandle.WaitAny(new[] {taskEvent, finishedEvent}, TimeSpan.FromSeconds(10));
                //Debug.WriteLine("Awaken...");
                lock (taskLock)
                {
                    while (hpOperations.Count > 0 && runOperations.Count < maxConcurrentOperations)
                    {
                        var operaion = hpOperations.Dequeue();
                        var id = Guid.NewGuid();
                        RunOperation(id, operaion);
                    }
                    while (operations.Count > 0 && runOperations.Count < maxConcurrentOperations)
                    {
                        var operaion = operations.Dequeue();
                        var id = Guid.NewGuid();
                        RunOperation(id, operaion);
                    }
                }
            }
        }

        private void RunOperation(Guid id, Func<Task> action)
        {
            runOperations.Add(id);
            //Debug.WriteLine($"Started {id}");

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var disp = AppHelpers.Dispatcher;
                    if (disp != null)
                    {
                        await disp.DispatchAsync(async () =>
                        {
                            try
                            {
                                await action();
                            }
                            catch (Exception ex)
                            {
                                DebugHelper.BreakOnError(ex);
                            }
                            finally
                            {
                                //Debug.WriteLine($"Finished {id}");
                                lock (taskLock)
                                {
                                    runOperations.Remove(id);
                                }
                                finishedEvent.Set();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            });
        }

        /// <summary>
        /// Занести в список.
        /// </summary>
        /// <param name="action">Действие.</param>
        public void Enqueue(Func<Task> action)
        {
            lock (taskLock)
            {
                operations.Enqueue(action);
            }
            taskEvent.Set();
        }

        /// <summary>
        /// Занести в список с высоким приоритетом.
        /// </summary>
        /// <param name="action">Действие.</param>
        public void EnqueueHp(Func<Task> action)
        {
            lock (taskLock)
            {
                hpOperations.Enqueue(action);
            }
            taskEvent.Set();
        }
    }
}