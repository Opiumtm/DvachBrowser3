using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Страница со ссылкой.
    /// </summary>
    public interface IBoardLinkPage
    {
        /// <summary>
        /// Получить ссылку на страницу.
        /// </summary>
        /// <returns>Ссылка на страницу.</returns>
        Task<BoardLinkBase> GetPageLink();
    }
}