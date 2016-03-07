using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Циклический асинхронный буфер.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public sealed class AsyncCycleBuffer<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> buffer;

        private readonly List<TKey> keys;

        private readonly object asyncLock = new object();

        private readonly int maxValues;

        private readonly Func<TKey, Task<TValue>> defaultFunc;

        public AsyncCycleBuffer(int maxValues, Func<TKey, Task<TValue>> defaultFunc = null, IEqualityComparer<TKey> equalityComparer = null)
        {
            this.maxValues = maxValues;
            this.defaultFunc = defaultFunc;
            buffer = new Dictionary<TKey, TValue>(equalityComparer ?? EqualityComparer<TKey>.Default);
            keys = new List<TKey>();
        }

        public async Task<TValue> GetValue(TKey key, Func<TKey, Task<TValue>> func = null)
        {
            lock (asyncLock)
            {
                if (buffer.ContainsKey(key))
                {
                    return buffer[key];
                }
            }
            var f = func ?? defaultFunc;
            if (f == null)
            {
                return default(TValue);
            }
            var result = await f(key);
            lock (asyncLock)
            {
                buffer[key] = result;
                keys.Add(key);
                while (keys.Count > maxValues)
                {
                    var k = keys[0];
                    keys.RemoveAt(0);
                    buffer.Remove(k);
                }
            }
            return result;
        }
    }
}