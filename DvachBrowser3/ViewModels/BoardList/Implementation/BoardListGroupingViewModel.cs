using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группировка.
    /// </summary>
    public sealed class BoardListGroupingViewModel : List<IBoardListBoardViewModel>, IBoardListBoardGroupingViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="name">Имя группы.</param>
        /// <param name="isFavorite">Избранное.</param>
        public BoardListGroupingViewModel(string name, bool isFavorite)
        {
            IsFavorite = isFavorite;
            Name = name;
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Избранные.
        /// </summary>
        public bool IsFavorite { get; private set; }

        /// <summary>
        /// Есть элементы.
        /// </summary>
        public bool HasItems => this.Count > 0;
   }
}