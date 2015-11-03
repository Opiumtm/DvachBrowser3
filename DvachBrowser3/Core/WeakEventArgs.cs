using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Аргумент слабо связанного события.
    /// </summary>
    public sealed class WeakEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public WeakEventArgs(object parameter)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// Параметр.
        /// </summary>
        public object Parameter { get; }

        /// <summary>
        /// Проверка на принадлежность к типу.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <returns>Результат проверки.</returns>
        public bool Is<T>()
        {
            return Parameter is T;
        }

        /// <summary>
        /// Приведение к типу.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <returns>Приведённый к типу параметр.</returns>
        public T As<T>()
        {
            return (T)Parameter;
        }
    }
}