using Windows.Storage;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат медиа.
    /// </summary>
    public interface IMediaResult
    {
        /// <summary>
        /// Временный файл.
        /// </summary>
        StorageFile TempFile { get; }

        /// <summary>
        /// MIME-тип.
        /// </summary>
        string MimeType { get; }
    }
}