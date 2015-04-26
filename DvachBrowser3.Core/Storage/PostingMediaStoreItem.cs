using Windows.Storage;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Данные из медиа хранилища.
    /// </summary>
    public struct PostingMediaStoreItem
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id;

        /// <summary>
        /// Файл.
        /// </summary>
        public StorageFile File;
    }
}