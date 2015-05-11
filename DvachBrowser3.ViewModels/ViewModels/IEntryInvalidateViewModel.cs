namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель, поддерживающая обновление данных при входе на страницу.
    /// </summary>
    public interface IEntryInvalidateViewModel
    {
        /// <summary>
        /// Вход на страницу.
        /// </summary>
        void OnPageEntry();
    }
}