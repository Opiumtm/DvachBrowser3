using System;
using System.Collections.Generic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Ключ категории.
    /// </summary>
    public struct BoardCategoryKey
    {
        /// <summary>
        /// Избранное.
        /// </summary>
        public readonly bool IsFavorite;

        /// <summary>
        /// Имя.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="isFavorite">Избранное.</param>
        /// <param name="name">Имя.</param>
        public BoardCategoryKey(bool isFavorite, string name)
        {
            IsFavorite = isFavorite;
            Name = name;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="model">Модель.</param>
        public BoardCategoryKey(IBoardListBoardViewModel model)
        {
            IsFavorite = model.IsFavorite;
            Name = model.Category;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="model">Модель.</param>
        public BoardCategoryKey(IBoardListBoardGroupingViewModel model)
        {
            IsFavorite = model.IsFavorite;
            Name = model.Name;
        }

        /// <summary>
        /// Сравнение на равенство.
        /// </summary>
        public static IEqualityComparer<BoardCategoryKey> EqualityComparer { get; } = new IsFavoriteNameEqualityComparer();

        /// <summary>
        /// Сравнение.
        /// </summary>
        public static IComparer<BoardCategoryKey> Comparer { get; } = new IsFavoriteNameComparer();

        private sealed class IsFavoriteNameEqualityComparer : IEqualityComparer<BoardCategoryKey>
        {
            public bool Equals(BoardCategoryKey x, BoardCategoryKey y)
            {
                return x.IsFavorite == y.IsFavorite && string.Equals(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(BoardCategoryKey obj)
            {
                unchecked
                {
                    return (obj.IsFavorite.GetHashCode()*397) ^ (obj.Name != null ? StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj.Name) : 0);
                }
            }
        }

        private sealed class IsFavoriteNameComparer : IComparer<BoardCategoryKey>
        {
            public int Compare(BoardCategoryKey x, BoardCategoryKey y)
            {
                if (x.IsFavorite && !y.IsFavorite)
                {
                    return -1;
                }
                if (!x.IsFavorite && y.IsFavorite)
                {
                    return 1;
                }
                return StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, y.Name);
            }
        }
    }
}