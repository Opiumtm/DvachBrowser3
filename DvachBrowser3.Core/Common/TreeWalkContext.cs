using System;
using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Контекст просмотра дерева.
    /// </summary>
    /// <typeparam name="T">Тип элемента дерева.</typeparam>
    /// <typeparam name="TApp">Тип результата.</typeparam>
    public class TreeWalkContext<T, TApp> : ITreeWalkContextBreak
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TreeWalkContext()
        {
            Functions = new List<TreeApplyFunc<T, TApp>>();
        }

        /// <summary>
        /// Источник.
        /// </summary>
        internal IEnumerable<T> Source { get; set; }

        /// <summary>
        /// Результат.
        /// </summary>
        internal TApp Result { get; set; }

        /// <summary>
        /// Функции применения результата.
        /// </summary>
        internal List<TreeApplyFunc<T, TApp>> Functions { get; private set; }

        /// <summary>
        /// Функция применения результата по умолчанию.
        /// </summary>
        internal Func<T, TApp, TApp> DefaultApply { get; set; }

        /// <summary>
        /// Функция получения дочерних элементов по умолчанию.
        /// </summary>
        internal Func<T, IEnumerable<T>> DefaultGetChildren { get; set; }

        /// <summary>
        /// Прервать обработку.
        /// </summary>
        public bool IsBreak { get; set; }
    }
}