namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения данных.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Тред.
        /// </summary>
        IThreadStorageService Thread { get; }

        /// <summary>
        /// Борды.
        /// </summary>
        IBoardStorageService Board { get; set; }

        /// <summary>
        /// Постинг.
        /// </summary>
        IPostingStorageService Posting { get; }

        /// <summary>
        /// Файлы постинга.
        /// </summary>
        IPostingFilesStorageService PostingFiles { get; }

        /// <summary>
        /// Медиа файлы.
        /// </summary>
        IMediaStorageService Media { get; set; }

        /// <summary>
        /// Предпросмотр.
        /// </summary>
        IMediaStorageService ThumbnailMedia { get; set; }
    }
}