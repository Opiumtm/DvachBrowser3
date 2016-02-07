using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Обновление тайла треда.
    /// </summary>
    public interface IThreadTileUpdater
    {
        /// <summary>
        /// Обновить данные.
        /// </summary>
        /// <param name="collection">Коллекция.</param>
        /// <returns>Короткая информация.</returns>
        ShortThreadInfo UpdateData(ThreadLinkCollection collection);
    }
}