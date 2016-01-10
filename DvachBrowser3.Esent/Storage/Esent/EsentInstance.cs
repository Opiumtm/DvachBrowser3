using System;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Экземпляр.
    /// </summary>
    internal sealed class EsentInstance : IEsentInstance
    {
        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public EsentInstance(Action onDispose, Instance instance)
        {
            OnDispose = onDispose;
            Instance = instance;
        }

        /// <summary>
        /// Экземпляр.
        /// </summary>
        public Instance Instance { get; }

        /// <summary>
        /// Действие по завершению.
        /// </summary>
        private Action OnDispose;
    }
}