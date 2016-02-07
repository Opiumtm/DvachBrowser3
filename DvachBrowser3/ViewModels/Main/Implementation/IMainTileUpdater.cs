using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Обновление основного тайла.
    /// </summary>
    public interface IMainTileUpdater
    {
        /// <summary>
        /// Избранные изменились.
        /// </summary>
        /// <param name="favorites">Избранные.</param>
        void FavoritesChanged(ThreadLinkCollection favorites);

        /// <summary>
        /// Изменились посещённые.
        /// </summary>
        /// <param name="visited">Посещённые.</param>
        void VisitedChanged(ThreadLinkCollection visited);
    }
}