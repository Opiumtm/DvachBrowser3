using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Циклический кэш.
    /// </summary>
    internal sealed class CycleCache<TKey, T>
    {
        private readonly int maxSize;

        private readonly List<TKey> values = new List<TKey>();

        private readonly Dictionary<TKey, T> valueDic;

        private readonly IEqualityComparer<TKey> comparer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="maxSize">Максимальный размер.</param>
        /// <param name="comparer">Средство сравнения.</param>
        public CycleCache(int maxSize, IEqualityComparer<TKey> comparer = null)
        {
            this.maxSize = maxSize;
            this.comparer = comparer ?? EqualityComparer<TKey>.Default;
            this.valueDic = new Dictionary<TKey, T>(this.comparer);
        }

        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="valueFunc">Функция получения значения.</param>
        /// <returns>Значение.</returns>
        public T GetValue(TKey key, Func<T> valueFunc)
        {
            if (valueDic.ContainsKey(key))
            {
                return valueDic[key];
            }
            var value = valueFunc();
            values.Insert(0, key);
            valueDic[key] = value;
            while (values.Count > maxSize && values.Count > 0)
            {
                var toDelete = values[values.Count - 1];
                values.RemoveAt(values.Count - 1);
                valueDic.Remove(toDelete);
            }
            return value;
        }

        /// <summary>
        /// Получить и удалить значение.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="valueFunc">Функция получения значения.</param>
        /// <returns>Значение.</returns>
        public T ExtractValue(TKey key, Func<T> valueFunc)
        {
            if (valueDic.ContainsKey(key))
            {
                var result = valueDic[key];
                values.RemoveAll(e => comparer.Equals(e, key));
                valueDic.Remove(key);
                return result;
            }
            return valueFunc();
        }
    }
}