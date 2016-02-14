using System;
using System.ComponentModel;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Тайл основной модели.
    /// </summary>
    public interface IMainTileViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Данные тайла.
        /// </summary>
        ICommonTileViewModel TileData { get; }

        /// <summary>
        /// Можно добавлять в избранные.
        /// </summary>
        bool CanAddToFavorites { get; }
        
        /// <summary>
        /// Можно убрать из избранных.
        /// </summary>
        bool CanRemoveFromFavorites { get; }

        /// <summary>
        /// Находится в избранных.
        /// </summary>
        bool IsFavorite { get; }

        /// <summary>
        /// Добавить в избранное.
        /// </summary>
        void AddToFavorites();

        /// <summary>
        /// Убрать из избранного.
        /// </summary>
        void RemoveFromFavorites();

        /// <summary>
        /// Перейти по ссылке с тайла.
        /// </summary>
        void Navigate();

        /// <summary>
        /// Можно удалить.
        /// </summary>
        bool CanDelete { get; }

        /// <summary>
        /// Удалить.
        /// </summary>
        void Delete();

        /// <summary>
        /// Можно копировать ссылку.
        /// </summary>
        bool CanCopyLink { get; }

        /// <summary>
        /// Копировать ссылку в клипборд.
        /// </summary>
        void CopyLink();

        /// <summary>
        /// Идентификатор апдейта.
        /// </summary>
        ThreadListUpdateId UpdateId { get; }
    }
}