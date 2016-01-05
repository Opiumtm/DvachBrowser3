using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Ограничение на одновременное количество тасков.
    /// </summary>
    public sealed class AsyncConcurrencySemaphore : IAsyncLock
    {
        private readonly Queue<TaskCompletionSource<bool>> waiters = new Queue<TaskCompletionSource<bool>>();

        private static readonly Task CompletedTask = Task.FromResult(true);

        private int count;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="maxCount">Максимальное количество параллельных тасков.</param>
        public AsyncConcurrencySemaphore(int maxCount)
        {
            this.count = maxCount >= 1 ? maxCount : 1;
        }

        /// <summary>
        /// Войти.
        /// </summary>
        /// <returns>Таск.</returns>
        public Task Enter()
        {
            TaskCompletionSource<bool> tcs = null;
            lock (waiters)
            {
                if (count > 0)
                {
                    count = count - 1;
                }
                else
                {
                    tcs = new TaskCompletionSource<bool>();
                    waiters.Enqueue(tcs);
                }
            }
            return tcs != null ? tcs.Task : CompletedTask;
        }

        /// <summary>
        /// Выйти.
        /// </summary>
        public void Leave()
        {
            TaskCompletionSource<bool> tcs = null;
            lock (waiters)
            {
                if (waiters.Count > 0)
                {
                    tcs = waiters.Dequeue();
                }
                else
                {
                    count = count + 1;
                }
            }
            tcs?.TrySetResult(true);
        }

        public async Task<IDisposable> Lock()
        {
            await Enter();
            return new Section { Parent = this };
        }

        private struct Section : IDisposable
        {
            public AsyncConcurrencySemaphore Parent;

            public void Dispose()
            {
                Parent.Leave();
            }
        }
    }
}