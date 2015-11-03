using System;
using System.Collections.Generic;
using System.Linq;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощник коллекций.
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// Обновить коллекцию.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <typeparam name="TSrc">Тип исходного элемента.</typeparam>
        /// <typeparam name="TId">Тип идентификатора.</typeparam>
        /// <param name="items">Элементы.</param>
        /// <param name="result">Фильтрованная коллекция.</param>
        /// <param name="createFunc">Функция создания элемента.</param>
        /// <param name="idFunc">Функция идентификатора.</param>
        /// <param name="idFunc2">Функция идентификатора 2.</param>
        /// <param name="comparer">Сравнение идентификаторов.</param>
        public static void UpdateElements<T, TSrc, TId>(IEnumerable<TSrc> items, IList<T> result, Func<TSrc, T> createFunc, Func<T, TId> idFunc, Func<TSrc, TId> idFunc2, IEqualityComparer<TId> comparer = null) where T : class
        {
            var cmp = comparer ?? EqualityComparer<TId>.Default;
            var visited = new HashSet<TId>(cmp);
            foreach (var item in items)
            {
                var id1 = idFunc2(item);
                visited.Add(id1);
                var existing = result.FirstOrDefault(r => cmp.Equals(id1, idFunc(r)));
                if (existing == null)
                {
                    result.Add(createFunc(item));
                }
            }
            foreach (var item in result.ToArray())
            {
                if (!visited.Contains(idFunc(item)))
                {
                    result.Remove(item);
                }
            }
        }
    }

    /// <summary>
    /// Класс-помощник для 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSrc"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public sealed class SortedCollectionUpdateHelper<T, TSrc, TId>
    {
        private readonly IComparer<TId> idComparer;

        private readonly IEqualityComparer<TId> eqComparer;

        private readonly Func<TSrc, TId> srcIdFunc;

        private readonly Func<T, TId> idFunc;

        private readonly Func<TSrc, T> createFunc;

        private readonly Func<TSrc, T, bool> compareFunc;

        private readonly IList<TSrc> newData;

        private readonly IList<T> originalData;

        private readonly Action<T, TSrc> updateAction;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="eqComparer">Сравнение ключей на равенство.</param>
        /// <param name="idComparer">Сравнение ключей.</param>
        /// <param name="srcIdFunc">Получение ключа.</param>
        /// <param name="idFunc">Получение ключа.</param>
        /// <param name="createFunc">Создание элемента.</param>
        /// <param name="compareFunc">Сравнение элементов.</param>
        /// <param name="newData">Новые данные.</param>
        /// <param name="originalData">Оригинальные данные.</param>
        public SortedCollectionUpdateHelper(IEqualityComparer<TId> eqComparer, IComparer<TId> idComparer, Func<TSrc, TId> srcIdFunc, Func<T, TId> idFunc, Func<TSrc, T> createFunc, Func<TSrc, T, bool> compareFunc, Action<T, TSrc> updateAction, IList<TSrc> newData, IList<T> originalData)
        {
            this.eqComparer = eqComparer;
            this.idComparer = idComparer;
            this.srcIdFunc = srcIdFunc;
            this.idFunc = idFunc;
            this.createFunc = createFunc;
            this.compareFunc = compareFunc;
            this.newData = newData;
            this.originalData = originalData;
            this.updateAction = updateAction ?? new Action<T, TSrc>((o, s) => {});
        }

        /// <summary>
        /// Получить обновление.
        /// </summary>
        /// <returns>Обновление.</returns>
        public SortedCollectionUpdate<T> GetUpdate()
        {
            var ids = new HashSet<TId>(eqComparer);
            var srcDic = new Dictionary<TId, TSrc>(eqComparer);
            foreach (var s in newData)
            {
                var id = srcIdFunc(s);
                srcDic[id] = s;
                ids.Add(id);
            }
            var origDic = new Dictionary<TId, T>(eqComparer);
            foreach (var o in originalData)
            {
                var id = idFunc(o); 
                origDic[id] = o;
                ids.Add(id);
            }
            var result = new Dictionary<TId, SortedCollectionDiff<T>>(eqComparer);
            foreach (var id in ids)
            {
                if (srcDic.ContainsKey(id) && origDic.ContainsKey(id))
                {
                    var s = srcDic[id];
                    var o = origDic[id];
                    if (compareFunc(s, o))
                    {
                        result[id] = new SortedCollectionDiff<T>()
                        {
                            State = SortedCollectionDiffState.None,
                            Value = default(T),
                            UpdateAction = () => updateAction(o, s)
                        };
                    }
                    else
                    {
                        result[id] = new SortedCollectionDiff<T>()
                        {
                            State = SortedCollectionDiffState.Change,
                            Value = createFunc(s)
                        };
                    }
                }
                else if (srcDic.ContainsKey(id))
                {
                    var s = srcDic[id];
                    result[id] = new SortedCollectionDiff<T>()
                    {
                        State = SortedCollectionDiffState.Add,
                        Value = createFunc(s)
                    };                    
                }
                else if (origDic.ContainsKey(id))
                {
                    result[id] = new SortedCollectionDiff<T>()
                    {
                        State = SortedCollectionDiffState.Delete,
                        Value = default(T)
                    };
                }
            }
            return new SortedCollectionUpdate<T>(originalData, result.OrderBy(kv => kv.Key, idComparer).Select(kv => kv.Value).ToList());
        }
    }

    /// <summary>
    /// Данные для обновления сортированной коллекции.
    /// </summary>
    /// <typeparam name="T">Тип элемента.</typeparam>
    public sealed class SortedCollectionUpdate<T>
    {
        private readonly IList<T> original;

        private readonly IList<SortedCollectionDiff<T>> diff;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="original">Оригинал.</param>
        /// <param name="diff">Изменение.</param>
        public SortedCollectionUpdate(IList<T> original, IList<SortedCollectionDiff<T>> diff)
        {
            this.original = original;
            this.diff = diff;
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        public void Update()
        {
            int counter = 0;
            foreach (var d in diff)
            {
                switch (d.State)
                {
                    case SortedCollectionDiffState.None:
                        if (d.UpdateAction != null) d.UpdateAction();
                        counter++;
                        break;
                    case SortedCollectionDiffState.Change:
                        if (counter < original.Count)
                        {
                            original[counter] = d.Value;
                        }
                        counter++;
                        break;
                    case SortedCollectionDiffState.Delete:
                        if (counter < original.Count)
                        {
                            original.RemoveAt(counter);
                        }
                        break;
                    case SortedCollectionDiffState.Add:
                        if (counter < original.Count)
                        {
                            original.Insert(counter, d.Value);                            
                        }
                        else
                        {
                            original.Add(d.Value);
                        }
                        counter++;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Разница между коллекциями.
    /// </summary>
    /// <typeparam name="T">Тип элемента.</typeparam>
    public struct SortedCollectionDiff<T>
    {
        /// <summary>
        /// Новый элемент.
        /// </summary>
        public T Value;

        /// <summary>
        /// Состояние.
        /// </summary>
        public SortedCollectionDiffState State;

        /// <summary>
        /// Обновить.
        /// </summary>
        public Action UpdateAction;
    }

    public enum SortedCollectionDiffState
    {
        /// <summary>
        /// Нет.
        /// </summary>
        None,

        /// <summary>
        /// Изменить.
        /// </summary>
        Change,

        /// <summary>
        /// Добавить.
        /// </summary>
        Add,

        /// <summary>
        /// Удалить.
        /// </summary>
        Delete
    }
}