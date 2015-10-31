﻿using System;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления борды в списке.
    /// </summary>
    public sealed class BoardListBoardDataViewModel : ViewModelBase, IBoardListBoardViewModel
    {
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
        public BoardListBoardDataViewModel(BoardLinkBase link, string displayName)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            Link = link;
            DisplayName = displayName ?? "";
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
        public string DisplayName { get; private set; }

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