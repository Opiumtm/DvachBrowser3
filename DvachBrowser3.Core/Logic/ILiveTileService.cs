using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис обновления живых плиток.
    /// </summary>
    public interface ILiveTileService
    {
        /// <summary>
        /// Обновить тайл с информацией об избранных тредах.
        /// </summary>
        /// <param name="favorites">Избранные треды.</param>
        /// <returns>Таск.</returns>
        Task UpdateFavoritesTile(LinkCollection favorites);
    }
}