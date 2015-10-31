using System;
using System.Linq;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления борды в списке.
    /// </summary>
    public sealed class BoardListBoardViewModel : ViewModelBase, IBoardListBoardViewModel
    {
        private readonly BoardReference boardRef;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="boardRef">Ссылка на борду.</param>
        public BoardListBoardViewModel(BoardReference boardRef)
        {
            if (boardRef == null) throw new ArgumentNullException(nameof(boardRef));
            this.boardRef = boardRef;
        }

        public BoardLinkBase Link => boardRef.Link;

        /// <summary>
        /// Короткое имя.
        /// </summary>
        public string ShortName => boardRef.ShortName;

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string DisplayName => boardRef.DisplayName;

        /// <summary>
        /// Движок.
        /// </summary>
        public string Engine => BoardListBoardViewModelsHelper.GetEngineName(Link?.Engine);

        /// <summary>
        /// Ресурс.
        /// </summary>
        public string Resource => BoardListBoardViewModelsHelper.GetResourceName(Link?.Engine);

        /// <summary>
        /// Фильтровать.
        /// </summary>
        /// <param name="filterString">Строка фильтра.</param>
        /// <returns>true, если нужно отобразить.</returns>
        public bool Filter(string filterString)
        {
            return BoardListBoardViewModelsHelper.Filter(this, filterString);
        }
    }
}