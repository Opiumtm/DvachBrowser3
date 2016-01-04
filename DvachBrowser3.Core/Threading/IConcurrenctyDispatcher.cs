using System;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Диспетчер параллельного доступа.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    public interface IConcurrenctyDispatcher<T>
    {
        /// <summary>
        /// Запланировать действие.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        Task<T> QueueAction(Func<Task<T>> action);
    }
}