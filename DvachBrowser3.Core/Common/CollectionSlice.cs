using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Разбитая часть коллекции.
    /// </summary>
    /// <typeparam name="T">Тип элемента.</typeparam>
    public class CollectionSlice<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sliceNumber">Номер.</param>
        /// <param name="slice">Часть коллекции.</param>
        public CollectionSlice(int sliceNumber, ICollection<T> slice)
        {
            SliceNumber = sliceNumber;
            Slice = slice;
        }

        /// <summary>
        /// Номер.
        /// </summary>
        public int SliceNumber { get; private set; }

        /// <summary>
        /// Часть коллекции.
        /// </summary>
        public ICollection<T> Slice { get; private set; }
    }
}