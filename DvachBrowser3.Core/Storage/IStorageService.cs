
namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения данных.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Директории данных.
        /// </summary>
        ICacheFolderInfo[] CacheFolders { get; }
    }
}