using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace DvachBrowser3
{
    /// <summary>
    /// Менеджер последовательного доступа к ресурсу.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public sealed class SerializedAccessManager<T> : IConcurrenctyDispatcher<T>
    {
        private readonly AsyncLock asyncLock = new AsyncLock();

        /// <summary>
        /// Запланировать действие.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public async Task<T> QueueAction(Func<Task<T>> action)
        {
            using (await asyncLock.Lock())
            {
                return await action();
            }
        }
    }
}