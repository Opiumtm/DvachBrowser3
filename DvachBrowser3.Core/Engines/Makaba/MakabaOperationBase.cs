using System;
using System.Threading.Tasks;

namespace DvachBrowser3.Engines.Makaba
{
    public abstract class MakabaOperationBase<T, TParam> : IEngineOperationsWithProgress<T, EngineProgress>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected MakabaOperationBase(TParam parameter, IServiceProvider services)
        {
            Parameter = parameter;
            Services = services;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <returns>Таск.</returns>
        public abstract Task<T> Complete();

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Прогресс.
        /// </summary>
        /// <param name="e">Событие прогресса.</param>
        protected virtual void OnProgress(EngineProgress e)
        {
            EventHandler<EngineProgress> handler = Progress;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Параметр.
        /// </summary>
        public TParam Parameter { get; private set; }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }
    }
}