using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HtmlAgilityPack;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник общего назначения.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Проверка на равенство строк без учёта регистра.
        /// </summary>
        /// <param name="a">Первая строка.</param>
        /// <param name="b">Вторая строка.</param>
        /// <returns>Результат проверки.</returns>
        public static bool EqualsNc(this string a, string b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Пройтись по шаблону.
        /// </summary>
        /// <typeparam name="T1">Тип исходного элемента.</typeparam>
        /// <typeparam name="T2">Тип элемента-результата.</typeparam>
        /// <param name="value">Значение.</param>
        /// <param name="check">Проверка</param>
        /// <param name="getNext">Получить следующий элемент.</param>
        /// <returns>Результат.</returns>
        public static T2 WalkTemplate<T1, T2>(this T1 value, Func<T1, bool> check, Func<T1, T2> getNext)
            where T1 : class
            where T2 : class
        {
            if (value == null)
            {
                return null;
            }
            if (!check(value))
            {
                return null;
            }
            return getNext(value);
        }

        /// <summary>
        /// Пройтись по шаблону.
        /// </summary>
        /// <typeparam name="T1">Тип исходного элемента.</typeparam>
        /// <typeparam name="T2">Тип элемента-результата.</typeparam>
        /// <param name="value">Значение.</param>
        /// <param name="getNext">Получить следующий элемент.</param>
        /// <returns>Результат.</returns>
        public static T2 WalkTemplate<T1, T2>(this T1 value, Func<T1, T2> getNext)
            where T1 : class
            where T2 : class
        {
            if (value == null)
            {
                return null;
            }
            return getNext(value);
        }

        /// <summary>
        /// Найти первый не текстовый дочерний элемент.
        /// </summary>
        /// <param name="node">Элемент.</param>
        /// <returns>Первый не текстовый дочерний элемент.</returns>
        public static HtmlNode FirstNonTextChild(this HtmlNode node)
        {
            if (node == null)
            {
                return null;
            }
            if (!node.HasChildNodes)
            {
                return null;
            }
            return node.ChildNodes.FirstOrDefault(c => c.NodeType == HtmlNodeType.Element);
        }

        /// <summary>
        /// Создать контекст рекурсивного обхода дерева.
        /// </summary>
        /// <typeparam name="T">Тип элемента дерева.</typeparam>
        /// <typeparam name="TApp">Тип результата.</typeparam>
        /// <param name="source">Источник.</param>
        /// <param name="result">Результат.</param>
        /// <returns>Контекст.</returns>
        public static TreeWalkContext<T, TApp> TreeWalk<T, TApp>(this IEnumerable<T> source, TApp result)
        {
            return new TreeWalkContext<T, TApp>()
            {
                Source = source,
                Result = result,
                DefaultApply = (i, r) => r,
                DefaultGetChildren = i => null
            };
        }

        /// <summary>
        /// Создать сопоставление для рекурсивного обхода дерева.
        /// </summary>
        /// <typeparam name="T">Тип элемента дерева.</typeparam>
        /// <typeparam name="TApp">Тип результата.</typeparam>
        /// <param name="context">Контекст.</param>
        /// <param name="ifFunc">Функция сопоставления.</param>
        /// <param name="applyFunc">Функция применения результата.</param>
        /// <param name="getChildrenFunc">Функция получения дочерних элементов.</param>
        /// <returns>Контекст.</returns>
        public static TreeWalkContext<T, TApp> If<T, TApp>(this TreeWalkContext<T, TApp> context, Func<T, bool> ifFunc,
                                                           Func<T, TApp, TApp> applyFunc = null,
                                                           Func<T, IEnumerable<T>> getChildrenFunc = null)
        {
            context.Functions.Add(new TreeApplyFunc<T, TApp>()
            {
                Apply = applyFunc,
                GetChildren = getChildrenFunc,
                If = ifFunc ?? (v => true),
                IsElse = ifFunc == null
            });
            return context;
        }

        /// <summary>
        /// Создать сопоставление для рекурсивного обхода дерева для прочих случаев.
        /// </summary>
        /// <typeparam name="T">Тип элемента дерева.</typeparam>
        /// <typeparam name="TApp">Тип результата.</typeparam>
        /// <param name="context">Контекст.</param>
        /// <param name="applyFunc">Функция применения результата.</param>
        /// <param name="getChildrenFunc">Функция получения дочерних элементов.</param>
        /// <returns>Контекст.</returns>
        public static TreeWalkContext<T, TApp> Else<T, TApp>(this TreeWalkContext<T, TApp> context,
                                                             Func<T, TApp, TApp> applyFunc = null,
                                                             Func<T, IEnumerable<T>> getChildrenFunc = null)
        {
            return context.If(null, applyFunc, getChildrenFunc);
        }

        /// <summary>
        /// Установить функцию получения дочерних элементов по умолчанию.
        /// </summary>
        /// <typeparam name="T">Тип элемента дерева.</typeparam>
        /// <typeparam name="TApp">Тип результата.</typeparam>
        /// <param name="context">Контекст.</param>
        /// <param name="getChildrenFunc">Функция получения дочерних элементов.</param>
        /// <returns>Контекст.</returns>
        public static TreeWalkContext<T, TApp> GetChildren<T, TApp>(this TreeWalkContext<T, TApp> context, Func<T, IEnumerable<T>> getChildrenFunc)
        {
            context.DefaultGetChildren = getChildrenFunc;
            return context;
        }

        /// <summary>
        /// Установить функцию применения результата по умолчанию.
        /// </summary>
        /// <typeparam name="T">Тип элемента дерева.</typeparam>
        /// <typeparam name="TApp">Тип результата.</typeparam>
        /// <param name="context">Контекст.</param>
        /// <param name="applyFunc">Функция применения результата.</param>
        /// <returns>Контекст.</returns>
        public static TreeWalkContext<T, TApp> Apply<T, TApp>(this TreeWalkContext<T, TApp> context, Func<T, TApp, TApp> applyFunc)
        {
            context.DefaultApply = applyFunc;
            return context;
        }

        /// <summary>
        /// Выполнить проход по дереву.
        /// </summary>
        /// <typeparam name="T">Тип элемента дерева.</typeparam>
        /// <typeparam name="TApp">Тип результата.</typeparam>
        /// <param name="context">Контекст.</param>
        /// <returns>Результат.</returns>
        public static TApp Run<T, TApp>(this TreeWalkContext<T, TApp> context)
        {
            WalkTree(context, context.Source, context.Result);
            return context.Result;
        }

        private static void WalkTree<T, TApp>(TreeWalkContext<T, TApp> context, IEnumerable<T> elements,
                                              TApp currentResult)
        {
            if (elements == null || context.IsBreak)
            {
                return;
            }
            foreach (var item in elements)
            {
                if (context.IsBreak)
                {
                    break;
                }
                var applyFunc = context.Functions.Where(f => !f.IsElse).FirstOrDefault(f => f.If(item)) ??
                                context.Functions.Where(f => f.IsElse).FirstOrDefault(f => f.If(item));
                if (applyFunc != null)
                {
                    var newResult = (applyFunc.Apply ?? context.DefaultApply)(item, currentResult);
                    var children = (applyFunc.GetChildren ?? context.DefaultGetChildren)(item);
                    WalkTree(context, children, newResult);
                }
            }
        }

        /// <summary>
        /// Разбить перечисление на части.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <param name="src">Перечисление-источник.</param>
        /// <param name="sliceSize">Размер части.</param>
        /// <returns>Разбитое на части перечисление.</returns>
        public static IEnumerable<CollectionSlice<T>> Slice<T>(this ICollection<T> src, int sliceSize)
        {
            var counter = 0;
            var slice = src.Take(sliceSize).ToArray();
            while (slice.Any())
            {
                yield return new CollectionSlice<T>(counter, slice);
                counter++;
                slice = src.Skip(counter * sliceSize).Take(sliceSize).ToArray();
            }
        }

        /// <summary>
        /// Преобразовать иерархию в список.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="nodes">Ноды.</param>
        /// <param name="getChildren">Получение дочерних элементов.</param>
        /// <returns></returns>
        public static IEnumerable<T> FlatHierarchy<T>(this IEnumerable<T> nodes, Func<T, IEnumerable<T>> getChildren)
        {
            return (nodes ?? new T[0]).SelectMany(c => FlatHierarchy(c, getChildren));
        }

        /// <summary>
        /// Преобразовать иерархию в список.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="node">Нода.</param>
        /// <param name="getChildren">Получение дочерних элементов.</param>
        /// <returns></returns>
        public static IEnumerable<T> FlatHierarchy<T>(this T node, Func<T, IEnumerable<T>> getChildren)
        {
            yield return node;
            foreach (var item in FlatHierarchy(getChildren(node), getChildren))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Попробовать отпарсить цифровое значение.
        /// </summary>
        /// <param name="src">Строка.</param>
        /// <param name="def">Значение по умолчанию.</param>
        /// <returns>Целое число.</returns>
        public static int TryParseWithDefault(this string src, int def = 0)
        {
            int result;
            if (int.TryParse(src, out result))
            {
                return result;
            }
            return def;
        }

        /// <summary>
        /// Сделать случайный порядок перечисления.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <param name="src">Исходное перечисление.</param>
        /// <returns>Результат.</returns>
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> src)
        {
            var random = new Random();
            return src.Select(item => new { element = item, order = random.Next() }).OrderBy(item => item.order).Select(item => item.element);
        }

        /// <summary>
        /// Удалить повторы.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <typeparam name="TKey">Тип ключа.</typeparam>
        /// <param name="src">Исходное перечисление.</param>
        /// <param name="keyFunc">Функция получения ключа.</param>
        /// <param name="comparer">Средство сравнения.</param>
        /// <returns>Результат.</returns>
        public static IEnumerable<T> Deduplicate<T, TKey>(this IEnumerable<T> src, Func<T, TKey> keyFunc, IEqualityComparer<TKey> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            return src.GroupBy(keyFunc, comparer).Select(a => a.First());
        }

        /// <summary>
        /// Получить последовательность с ключами.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <typeparam name="TKey">Тип ключа.</typeparam>
        /// <param name="src">Исходное перечисление.</param>
        /// <param name="keyFunc">Функция получения ключа.</param>
        /// <returns>Результат.</returns>
        public static IEnumerable<KeyValuePair<TKey, T>> WithKeys<T, TKey>(this IEnumerable<T> src, Func<T, TKey> keyFunc)
        {
            return src.Select(a => new KeyValuePair<TKey, T>(keyFunc(a), a));
        }

        /// <summary>
        /// Удалить повторы и сделать словарём.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <typeparam name="TKey">Тип ключа.</typeparam>
        /// <param name="src">Исходное перечисление.</param>
        /// <param name="keyFunc">Функция получения ключа.</param>
        /// <param name="comparer">Средство сравнения.</param>
        /// <returns>Результат.</returns>
        public static Dictionary<TKey, T> DeduplicateToDictionary<T, TKey>(this IEnumerable<T> src, Func<T, TKey> keyFunc, IEqualityComparer<TKey> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            return src.WithKeys(keyFunc).Deduplicate(a => a.Key, comparer).ToDictionary(a => a.Key, a => a.Value, comparer);
        }

        /// <summary>
        /// Удалить слэш в начале.
        /// </summary>
        /// <param name="src">Исходная строка.</param>
        /// <returns>Строка.</returns>
        public static string RemoveStartingSlash(this string src)
        {
            return src.StartsWith("/") ? src.Remove(0, 1) : src;
        }

        /// <summary>
        /// Разбить последовательность.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <param name="src">Источник.</param>
        /// <param name="count">На сколько разбивать.</param>
        /// <returns>Разбитая последовательность.</returns>
        public static IEnumerable<IGrouping<int, T>> Split<T>(this IEnumerable<T> src, int count = 100)
        {
            if (count <= 0)
            {
                count = 1;
            }
            return src.WithCounter().GroupBy(t => t.Key/count, t => t.Value);
        }

        /// <summary>
        /// Разбить последовательность.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <param name="src">Источник.</param>
        /// <param name="count">На сколько разбивать.</param>
        /// <returns>Разбитая последовательность.</returns>
        public static ILookup<int, T> SplitLookup<T>(this IEnumerable<T> src, int count = 100)
        {
            if (count <= 0)
            {
                count = 1;
            }
            return src.WithCounter().ToLookup(t => t.Key / count, t => t.Value);
        }

        /// <summary>
        /// Получить список вместе со счётчиком.
        /// </summary>
        /// <typeparam name="T">Тип элемента.</typeparam>
        /// <param name="src">Источник.</param>
        /// <returns>Последовательность со счётчиком.</returns>
        public static IEnumerable<KeyValuePair<int, T>> WithCounter<T>(this IEnumerable<T> src)
        {
            int i = 0;
            foreach (var t in src)
            {
                yield return new KeyValuePair<int, T>(i, t);
                i++;
            }
        }
    }
}