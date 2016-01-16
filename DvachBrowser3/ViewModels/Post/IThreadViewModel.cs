using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления треда.
    /// </summary>
    public interface IThreadViewModel : IPostCollectionViewModel, IStartableViewModelWithResume
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Тред обновлён.
        /// </summary>
        bool IsUpdated { get; }

        /// <summary>
        /// Была навигация назад.
        /// </summary>
        bool IsBackNavigatedToViewModel { get; set; }

        /// <summary>
        /// Синхронизировать данные.
        /// </summary>
        void Synchronize();

        /// <summary>
        /// Полностью перезагрузить.
        /// </summary>
        void FullReload();

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        void CheckForUpdates();

        /// <summary>
        /// Сбросить статус обновления.
        /// </summary>
        void CleanUpdated();
    }
}