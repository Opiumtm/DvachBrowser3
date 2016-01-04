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
        private readonly AsyncConcurrencySemaphore asyncLock;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="maxConcurrency">Максимальное количество параллельных задач.</param>
        public MaxConcurrencyAccessManager(int maxConcurrency)
        {
            asyncLock = new AsyncConcurrencySemaphore(maxConcurrency);
        }

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