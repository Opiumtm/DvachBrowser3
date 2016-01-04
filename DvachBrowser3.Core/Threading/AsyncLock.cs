using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Асинхронная критическая секция.
    /// </summary>
    public sealed class AsyncLock : IAsyncLock
    {
        private readonly Queue<TaskCompletionSource<bool>> waiters = new Queue<TaskCompletionSource<bool>>();

        private bool isWaiting;

        /// <summary>
        /// Зайти в секцию.
        /// </summary>
        /// <returns>Таск.</returns>
        public Task Enter()
        {
            lock (waiters)
            {
                if (isWaiting)
                {
                    var tcs = new TaskCompletionSource<bool>();
                    waiters.Enqueue(tcs);
                    return tcs.Task;
                }
                isWaiting = true;
                return Task.FromResult(true);
            }
        }

        /// <summary>
        /// Выйти из секции.
        /// </summary>
        public void Leave()
        {
            TaskCompletionSource<bool> tcs = null;
            lock (waiters)
            {
                if (waiters.Count == 0)
                {
                    isWaiting = false;
                }
                else
                {
                    tcs = waiters.Dequeue();
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
            public AsyncLock Parent;

            public void Dispose()
            {
                Parent.Leave();
            }
        }
    }
}