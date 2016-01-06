using System;
using System.Threading;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Асинхронное отложенное получение значения. 
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public sealed class AsyncLazy<T> : IAsyncLazy<T>
    {
        private readonly Func<Task<T>> factory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="factory">Фабрика значений.</param>
        public AsyncLazy(Func<Task<T>> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.factory = factory;
        }

        private T value;
        private int valueGot;
        private readonly AsyncLock valueLock = new AsyncLock();

        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        public async Task<T> GetValue()
        {
            if (Interlocked.CompareExchange(ref valueGot, 0, 0) == 0)
            {
                using (await valueLock.Lock())
                {
                    if (Interlocked.CompareExchange(ref valueGot, 0, 0) == 0)
                    {
                        value = await factory();
                        Interlocked.Exchange(ref valueGot, 1);
                    }
                }
            }
            return value;
        }
    }
}