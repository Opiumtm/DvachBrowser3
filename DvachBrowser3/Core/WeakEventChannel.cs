using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DvachBrowser3
{
    /// <summary>
    /// Канал слабо связанных событий.
    /// </summary>
    public sealed class WeakEventChannel : IWeakEventChannel
    {
        private readonly ConcurrentDictionary<Guid, WeakReference<IWeakEventCallback>> callbacks = new ConcurrentDictionary<Guid, WeakReference<IWeakEventCallback>>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор канала.</param>
        public WeakEventChannel(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор канала.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Зарегистрировать обратныйй вызов.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>Токен обратного вызова.</returns>
        public Guid AddCallback(IWeakEventCallback callback)
        {
            var token = Guid.NewGuid();
            callbacks.TryAdd(token, new WeakReference<IWeakEventCallback>(callback));
            CleanupReferences();
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
            }
            foreach (var callback in toCall)
            {
                try
                {
                    callback.ReceiveWeakEvent(sender, this, e);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
        }

        private int MaxCount = 100;

        private void CleanupReferences()
        {
            try
            {
                var cnt = callbacks.Count;
                var maxCnt = Interlocked.CompareExchange(ref MaxCount, 0, 0);
                if (cnt > maxCnt)
                {
                    DoCleanupReferences();
                    cnt = callbacks.Count;
                    Interlocked.Exchange(ref MaxCount, cnt + 100);
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private void DoCleanupReferences()
        {
            var references = callbacks.ToArray();
            foreach (var r in references)
            {
                IWeakEventCallback callback;
                if (!r.Value.TryGetTarget(out callback))
                {
                    WeakReference<IWeakEventCallback> wk;
                    callbacks.TryRemove(r.Key, out wk);
                }
            }
        }
    }
}