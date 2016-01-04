using System;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Контекст выполнения.
    /// </summary>
    public interface IThreadExecutionContext : IDisposable
    {
        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        Task Execute(Action action);

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="func">Функция.</param>
        /// <returns>Результат.</returns>
        Task<T> Execute<T>(Func<T> func);

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <param name="action">Действие.</param>
        /// <param name="param">Параметр.</param>
        /// <returns>Таск.</returns>
        Task Execute<TParam>(Action<TParam> action, TParam param);

        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="func">Функция.</param>
        /// <param name="param">Параметр.</param>
        /// <returns>Результат.</returns>
        Task<T> Execute<TParam, T>(Func<TParam, T> func, TParam param);
    }
}