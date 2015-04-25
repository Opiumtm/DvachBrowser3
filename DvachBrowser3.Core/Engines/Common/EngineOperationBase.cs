﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Базовый класс операции движка.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class EngineOperationBase<T, TParam> : IEngineOperationsWithProgress<T, EngineProgress>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected EngineOperationBase(TParam parameter, IServiceProvider services)
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

        private IAsyncInfo asyncInfo;

        protected IAsyncInfo Operation
        {
            get { return Interlocked.CompareExchange(ref asyncInfo, null, null); }
            set { Interlocked.Exchange(ref asyncInfo, value); }
        }

        /// <summary>
        /// Отменить.
        /// </summary>
        public virtual void Cancel()
        {
            try
            {
                var operation = Operation;
                if (operation != null)
                {
                    if (operation.Status == AsyncStatus.Started)
                    {
                        operation.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }

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