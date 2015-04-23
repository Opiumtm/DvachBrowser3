using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис хэша ссылок.
    /// </summary>
    public interface ILinkHashService
    {
        /// <summary>
        /// Получить хэш ссылки.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Хэш.</returns>
        string GetLinkHash(BoardLinkBase link);
    }
}