using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Канал слабо связанных событий.
    /// </summary>
    public sealed class WeakEventChannel : IWeakEventChannel
    {
        private ConcurrentDictionary<Guid, WeakReference<IWeakEventCallback>> callbacks = new ConcurrentDictionary<Guid, WeakReference<IWeakEventCallback>>();

        /// <summary>
        /// Зарегистрировать обратныйй вызов.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>Токен обратного вызова.</returns>
        public Guid AddCallback(IWeakEventCallback callback)
        {
            var token = Guid.NewGuid();
            callbacks.TryAdd(token, new WeakReference<IWeakEventCallback>(callback));
            return token;
        }

        /// <summary>
        /// Удалить обратный вызов.
        /// </summary>
        /// <param name="token">Токен.</param>
        public void RemoveCallback(Guid token)
        {
            WeakReference<IWeakEventCallback> reference;
            callbacks.TryRemove(token, out reference);
        }

        /// <summary>
        /// Вызвать событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        public void RaiseEvent(object sender, object e)
        {
            var references = callbacks.ToArray();
            var toCall = new List<IWeakEventCallback>();
            foreach (var r in references)
            {
                IWeakEventCallback callback;
                if (r.Value.TryGetTarget(out callback))
                {
                    toCall.Add(callback);
                }
                else
                {
                    WeakReference<IWeakEventCallback> wk;
                    callbacks.TryRemove(r.Key, out wk);
                }
            }
            foreach (var callback in toCall)
            {
                try
                {
                    callback.ReceiveWeakEvent(sender, e);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
        }
    }
}