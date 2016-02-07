using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    public struct ThreadListUpdateId
    {
        public string LinkHash;
        public DateTime SortDate;
        public BoardLinkBase Link;

        public static readonly IEqualityComparer<ThreadListUpdateId> EqualityComparer = new EqualityComparerObj();

        public static readonly IComparer<ThreadListUpdateId> Comparer = new ComparerObj();

        private class EqualityComparerObj : IEqualityComparer<ThreadListUpdateId>
        {
            /// <summary>
            /// Определяет, равны ли два указанных объекта.
            /// </summary>
            /// <returns>
            /// true, если указанные объекты равны; в противном случае — false.
            /// </returns>
            /// <param name="x">Первый сравниваемый объект типа <paramref name="T"/>.</param><param name="y">Второй сравниваемый объект типа <paramref name="T"/>.</param>
            public bool Equals(ThreadListUpdateId x, ThreadListUpdateId y)
            {
                return StringComparer.Ordinal.Equals(x.LinkHash, y.LinkHash);
            }

            /// <summary>
            /// Возвращает хэш-код указанного объекта.
            /// </summary>
            /// <returns>
            /// Хэш-код указанного объекта.
            /// </returns>
            /// <param name="obj">Объект <see cref="T:System.Object"/>, для которого необходимо возвратить хэш-код.</param><exception cref="T:System.ArgumentNullException">Тип параметра <paramref name="obj"/> является ссылочным типом и значение параметра <paramref name="obj"/> — null.</exception>
            public int GetHashCode(ThreadListUpdateId obj)
            {
                return StringComparer.Ordinal.GetHashCode(obj.LinkHash);
            }
        }

        private class ComparerObj : IComparer<ThreadListUpdateId>
        {
            /// <summary>
            /// Сравнение двух объектов и возврат значения, указывающего, является ли один объект меньшим, равным или большим другого.
            /// </summary>
            /// <returns>
            /// Знаковое целое число, которое определяет относительные значения параметров <paramref name="x"/> и <paramref name="y"/>, как показано в следующей таблице.Значение Значение Меньше нуляЗначение параметра <paramref name="x"/> меньше значения параметра <paramref name="y"/>.ZeroЗначения параметров <paramref name="x"/> и <paramref name="y"/> равны.Больше нуля.Значение <paramref name="x"/> больше значения <paramref name="y"/>.
            /// </returns>
            /// <param name="x">Первый сравниваемый объект.</param><param name="y">Второй сравниваемый объект.</param>
            public int Compare(ThreadListUpdateId x, ThreadListUpdateId y)
            {
                if (StringComparer.Ordinal.Equals(x.LinkHash, y.LinkHash))
                {
                    return 0;
                }
                // Обратный порядок сортировки!
                return Comparer<DateTime>.Default.Compare(y.SortDate, x.SortDate);
            }
        }
    }
}