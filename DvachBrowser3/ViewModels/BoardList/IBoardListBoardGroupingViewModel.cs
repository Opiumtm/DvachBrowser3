using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Группировка списка борд.
    /// </summary>
    public interface IBoardListBoardGroupingViewModel : IList<IBoardListBoardViewModel>
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Избранные.
        /// </summary>
        bool IsFavorite { get; }

        /// <summary>
        /// Есть элементы.
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        IStyleManager StyleManager { get; }
    }
}