using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
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
        /// Этот элемент.
        /// </summary>
        public IBoardListBoardViewModel This => this;

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
        /// Борда.
        /// </summary>
        public string Board => boardRef.ShortName;

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string DisplayName => boardRef.DisplayName;

        /// <summary>
        /// Изображение.
        /// </summary>
        public IImageSourceViewModel Image => null;

        /// <summary>
        /// Есть новые посты.
        /// </summary>
        public bool HasNewPosts => false;

        /// <summary>
        /// Новые посты.
        /// </summary>
        public int NewPosts => 0;

        /// <summary>
        /// Движок.
        /// </summary>
        public string Engine => BoardListBoardViewModelsHelper.GetEngineName(Link?.Engine);

        /// <summary>
        /// Ресурс.
        /// </summary>
        public string Resource => BoardListBoardViewModelsHelper.GetResourceName(Link?.Engine);

        /// <summary>
        /// Категория.
        /// </summary>
        public string Category => boardRef.Category ?? "Без категории";

        /// <summary>
        /// Цвет плитки.
        /// </summary>
        public Color TileBackgroundColor => BoardListBoardViewModelsHelper.GetResourceColor(Link?.Engine);

        /// <summary>
        /// Логотип.
        /// </summary>
        public ImageSource ResourceLogo => BoardListBoardViewModelsHelper.GetLogo(Link?.Engine);

        /// <summary>
        /// Фильтровать.
        /// </summary>
        /// <param name="filterString">Строка фильтра.</param>
        /// <returns>true, если нужно отобразить.</returns>
        public bool Filter(string filterString)
        {
            return BoardListBoardViewModelsHelper.Filter(this, filterString);
        }

        /// <summary>
        /// Избранное.
        /// </summary>
        public bool IsFavorite => false;

        /// <summary>
        /// Для взрослых.
        /// </summary>
        public bool IsAdult => boardRef.IsAdult;

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = new StyleManager();

        /// <summary>
        /// Номер треда.
        /// </summary>
        public int ThreadNumber => 0;
    }
}