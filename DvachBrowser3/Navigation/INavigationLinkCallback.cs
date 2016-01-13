using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Обработчик переходов по ссылке.
    /// </summary>
    public interface INavigationLinkCallback
    {
        /// <summary>
        /// Обработать событие по переходу по ссылке.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Событие.</param>
        /// <returns>true, если обработка произведена и действий по умолчанию не требуется.</returns>
        bool HandleNavigationLinkClick(object sender, LinkClickEventArgs e);
    }
}