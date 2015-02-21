using System;
using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Функция применения результата после сопоставления.
    /// </summary>
    /// <typeparam name="T">Тип элемента дерева.</typeparam>
    /// <typeparam name="TApp">Тип результата.</typeparam>
    public class TreeApplyFunc<T, TApp>
    {
        /// <summary>
        /// Функция сопоставления.
        /// </summary>
        internal Func<T, bool> If { get; set; }

        /// <summary>
        /// Функция применения результата.
        /// </summary>
        internal Func<T, TApp, TApp> Apply { get; set; }

        /// <summary>
        /// Функция получения дочерних элементов.
        /// </summary>
        internal Func<T, IEnumerable<T>> GetChildren { get; set; }

        /// <summary>
        /// Функция для прочих случаев.
        /// </summary>
        internal bool IsElse { get; set; }
    }
}