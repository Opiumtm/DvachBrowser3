using DvachBrowser3.Links;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Запрос на капчу.
    /// </summary>
    public interface ICatpchaQueryView
    {
        /// <summary>
        /// Результат запроса на капчу.
        /// </summary>
        event CaptchaQueryResultEventHandler CaptchaQueryResult;

        /// <summary>
        /// Загрузить капчу.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        void Load(BoardLinkBase link);
    }
}