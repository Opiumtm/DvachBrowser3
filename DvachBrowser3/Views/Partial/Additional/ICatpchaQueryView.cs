using System.ComponentModel;
using DvachBrowser3.Links;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Запрос на капчу.
    /// </summary>
    public interface ICatpchaQueryView : INotifyPropertyChanged
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

        /// <summary>
        /// Обновить.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Принять.
        /// </summary>
        void Accept();

        /// <summary>
        /// Можно загрузить.
        /// </summary>
        bool CanLoad { get; }
    }
}