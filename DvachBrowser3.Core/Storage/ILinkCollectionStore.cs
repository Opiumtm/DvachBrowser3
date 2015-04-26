using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище коллекций ссылок.
    /// </summary>
    public interface ILinkCollectionStore
    {
        /// <summary>
        /// Сохранить коллекцию ссылок.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveLinkCollection(LinkCollection data);

        /// <summary>
        /// Загрузить коллекцию ссылок.
        /// </summary>
        /// <returns>Коллекция ссылок.</returns>
        Task<LinkCollection> LoadLinkCollection();

        /// <summary>
        /// Загрузить коллекцию ссылок только для чтения.
        /// </summary>
        /// <returns>Коллекция ссылок.</returns>
        Task<LinkCollection> LoadLinkCollectionForReadOnly();
    }
}