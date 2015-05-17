using System;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сервис URI Youtube.
    /// </summary>
    public interface IYoutubeUriService
    {
        /// <summary>
        /// Получить URI для просмотра.
        /// </summary>
        /// <param name="youtubeId">Идентификатор.</param>
        /// <returns>URI.</returns>
        Uri GetViewUri(string youtubeId);

        /// <summary>
        /// Получить URI картинки предварительного просмотра.
        /// </summary>
        /// <param name="youtubeId">Идентификатор.</param>
        /// <returns>URI</returns>
        Uri GetThumbnailUri(string youtubeId);

        /// <summary>
        /// Получить URI для запуска.
        /// </summary>
        /// <param name="youtubeId">Идентификатор.</param>
        /// <returns>URI</returns>
        Uri GetLaunchApplicationUri(string youtubeId);

        /// <summary>
        /// Высота картинки.
        /// </summary>
        int ThumbnailHeight { get; }

        /// <summary>
        /// Ширина картинки.
        /// </summary>
        int ThumbnailWidth { get; }
    }
}