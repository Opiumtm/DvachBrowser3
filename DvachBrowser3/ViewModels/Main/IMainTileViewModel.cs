using System.ComponentModel;

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
        object TileData { get; }

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
        /// <returns></returns>
        bool Delete();
    }
}