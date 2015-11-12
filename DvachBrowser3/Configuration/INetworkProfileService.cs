using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Сервис профилей сети.
    /// </summary>
    public interface INetworkProfileService
    {
        /// <summary>
        /// Получить текущий профиль.
        /// </summary>
        INetworkProfile CurrentProfile { get; }

        /// <summary>
        /// Идентификатор текущего профиля.
        /// </summary>
        string CurrentProfileId { get; set; }

        /// <summary>
        /// Получить профиль по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Профиль.</returns>
        Task<INetworkProfile> GetProfileById(string id);

        /// <summary>
        /// Удалить профиль.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Задача.</returns>
        Task DeleteProfile(string id);

        /// <summary>
        /// Зарегистрировать профиль.
        /// </summary>
        /// <param name="profile">Профиль.</param>
        /// <returns>Задача.</returns>
        Task RegisterProfile(INetworkProfile profile);

        /// <summary>
        /// Перечислить профили.
        /// </summary>
        /// <returns>Идентификаторы профилей.</returns>
        IList<string> ListProfiles();

        /// <summary>
        /// Инициализировать.
        /// </summary>
        /// <returns>Задача.</returns>
        Task Initialize();
    }
}