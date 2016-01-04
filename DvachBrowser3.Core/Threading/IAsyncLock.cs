using System;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Объект асинхронной синхронизации доступа.
    /// </summary>
    public interface IAsyncLock
    {
        /// <summary>
        /// Войти.
        /// </summary>
        /// <returns>Таск.</returns>
        Task Enter();

        /// <summary>
        /// Выйти.
        /// </summary>
        void Leave();

        /// <summary>
        /// Зайти и получить объект завершения доступа.
        /// </summary>
        /// <returns>Объект завершения доступа.</returns>
        Task<IDisposable> Lock();
    }
}