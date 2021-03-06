﻿using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Предварительный просмотр треда.
    /// </summary>
    public interface IThreadPreviewViewModel : IPostCollectionViewModel
    {
        /// <summary>
        /// Ссылка на тред.
        /// </summary>
        BoardLinkBase ThreadLink { get; }

        /// <summary>
        /// Родительская модель.
        /// </summary>
        IBoardPageViewModel Parent { get; }

        /// <summary>
        /// Количство изображений.
        /// </summary>
        int ImageCount { get; }

        /// <summary>
        /// Пропущено изображений.
        /// </summary>
        int OmitImages { get; }

        /// <summary>
        /// Количество постов.
        /// </summary>
        int PostCount { get; }

        /// <summary>
        /// Пропущено постов.
        /// </summary>
        int OmitPosts { get; }

        /// <summary>
        /// Не просмотренных постов.
        /// </summary>
        int NotViewedPosts { get; }

        /// <summary>
        /// Есть не отображённые посты.
        /// </summary>
        bool HasNotViewedPosts { get; }

        /// <summary>
        /// Тред скрыт.
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Скрыть тред.
        /// </summary>
        void Hide();

        /// <summary>
        /// Пометить как прочитанное.
        /// </summary>
        void MarkAsRead();

        /// <summary>
        /// Добавить в избранное.
        /// </summary>
        Task AddToFavorites();
    }
}