using System;
using Microsoft.Isam.Esent.Interop;

namespace DvachBrowser3.Storage.Esent
{
    /// <summary>
    /// Ёкземпл€р.
    /// </summary>
    internal sealed class EsentInstance : IEsentInstance
    {
        /// <summary>
        /// ¬ыполн€ет определ€емые приложением задачи, св€занные с удалением, высвобождением или сбросом неуправл€емых ресурсов.
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Object"/>.
        /// </summary>
        public EsentInstance(Action onDispose, Instance instance)
        {
            OnDispose = onDispose;
            Instance = instance;
        }

        /// <summary>
        /// Ёкземпл€р.
        /// </summary>
        public Instance Instance { get; }

        /// <summary>
        /// ƒействие по завершению.
        /// </summary>
        private Action OnDispose;
    }
}