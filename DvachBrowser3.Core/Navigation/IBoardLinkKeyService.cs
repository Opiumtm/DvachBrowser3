using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис получения ключей для ссылок.
    /// </summary>
    public interface IBoardLinkKeyService
    {
        /// <summary>
        /// Получить ключ.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ключ.</returns>
        INavigationKey GetKey(BoardLinkBase link);
    }
}