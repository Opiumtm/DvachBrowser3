namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Профиль сети.
    /// </summary>
    public interface INetworkProfile
    {
        /// <summary>
        /// Стандартный.
        /// </summary>
        bool IsStandard { get; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Обновлять страницу борды при входе.
        /// </summary>
        bool UpdateBoardPageOnEntry { get; }

        /// <summary>
        /// Показывать баннер.
        /// </summary>
        bool ShowBanner { get; }

        /// <summary>
        /// Проверять избранное на изменения.
        /// </summary>
        bool CheckFavoritesForUpdates { get; }

        /// <summary>
        /// Авоматически обновлять после поста.
        /// </summary>
        bool AutoUpdateThreadAfterPost { get; }
    }
}