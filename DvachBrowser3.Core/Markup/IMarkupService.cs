using DvachBrowser3.Posting;

namespace DvachBrowser3.Markup
{
    /// <summary>
    /// Сервис разметки.
    /// </summary>
    public interface IMarkupService
    {
        /// <summary>
        /// Получить провайдер разметки.
        /// </summary>
        /// <param name="markupType">Тип разметки.</param>
        /// <returns>Провайдер.</returns>
        IMarkupProvider GetProvider(PostingMarkupType markupType);

        /// <summary>
        /// Зарегистрировать провайдер.
        /// </summary>
        /// <param name="markupProvider">Провайдер.</param>
        void RegisterProvider(IMarkupProvider markupProvider);
    }
}