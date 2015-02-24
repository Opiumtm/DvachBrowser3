using Windows.Storage;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Данные постинга медиа файла.
    /// </summary>
    public class MediaFilePostingData
    {
        /// <summary>
        /// Временный файл.
        /// </summary>
        public StorageFile TempFile { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }
    }
}