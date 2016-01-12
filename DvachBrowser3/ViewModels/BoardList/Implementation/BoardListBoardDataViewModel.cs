using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления борды в списке.
    /// </summary>
    public sealed class BoardListBoardDataViewModel : ViewModelBase, IBoardListBoardViewModel
    {
        /// <summary>
        /// Этот элемент.
        /// </summary>
        public IBoardListBoardViewModel This => this;

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; private set; }

        private string shortName;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="displayName">Отображаемое имя.</param>
        /// <param name="category">Категория.</param>
        /// <param name="isFavorite">Избранное.</param>
        /// <param name="isAdult">Для взрослых.</param>
        public BoardListBoardDataViewModel(BoardLinkBase link, string displayName, string category, bool isFavorite, bool isAdult)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            Link = link;
            DisplayName = displayName ?? "";
            Category = category ?? "";
            IsFavorite = isFavorite;
            IsAdult = isAdult;
        }

        /// <summary>
        /// Короткое имя.
        /// </summary>
        public string ShortName
        {
            get
            {
                if (shortName == null)
                {
                    shortName = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetBoardShortName(Link) ?? "";
                }
                return shortName;
            }
        }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string DisplayName { get; }

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
        public string Category { get; private set; }

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
        public bool IsFavorite { get; }

        public bool IsAdult { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => Shell.StyleManager;
    }
}