using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Средство сбора данных за промежуток времени.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="T">Тип данных.</typeparam>
    public sealed class TimePeriodDataAggregator<TKey, T>
    {
        private readonly TimeSpan aggregateTime;

        private readonly Func<KeyValuePair<TKey, T>[], Task> saveFunc;

        private readonly Dictionary<TKey, T> dataDic;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="aggregateTime">Время сбора данных.</param>
        /// <param name="saveFunc">Функция сохранения.</param>
        /// <param name="keyComparer">Средство сравнения ключей.</param>
        public TimePeriodDataAggregator(TimeSpan aggregateTime, Func<KeyValuePair<TKey, T>[], Task> saveFunc, IEqualityComparer<TKey> keyComparer = null)
        {
            if (saveFunc == null) throw new ArgumentNullException(nameof(saveFunc));
            this.aggregateTime = aggregateTime;
            this.saveFunc = saveFunc;
            this.dataDic = new Dictionary<TKey, T>(keyComparer ?? EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="data">Данные.</param>
        public void Push(TKey key, T data)
        {
            lock (dataDic)
            {
                dataDic[key] = data;
            }
            SaverFunc();
        }

        private int isSaving;

        private void SaverFunc()
        {
            if (Interlocked.Exchange(ref isSaving, 1) == 0)
            {
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Task.Delay(aggregateTime);
                    }
                    finally
                    {
                        Interlocked.Exchange(ref isSaving, 0);
                    }
                    KeyValuePair<TKey, T>[] toSave;
                    lock (dataDic)
                    {
                        toSave = dataDic.ToArray();
                        dataDic.Clear();
                    }
                    await saveFunc(toSave);
                });
            }
        }
    }
}