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

        /// <summary>
        /// Обновлять страницу треда при входе.
        /// </summary>
        bool UpdateThreadPageOnEntry { get; }

        /// <summary>
        /// Автоматически загружать предварительный просмотр изображений.
        /// </summary>
        bool AutoLoadImageThumbnails { get; }

        /// <summary>
        /// Проверять тред на апдейты (интервал проверки в секундах, null - не проверять).
        /// </summary>
        double? CheckThreadForUpdatesSec { get; }

        /// <summary>
        /// Проверять на обновления вместо загрузки.
        /// </summary>
        bool CheckForUpdatesInsteadOfLoad { get; }

        /// <summary>
        /// Предпочитать частичную загрузку.
        /// </summary>
        bool PreferPartialLoad { get; }

        /// <summary>
        /// Предупреждать о полной перезагрузке.
        /// </summary>
        bool WarningFullReload { get; }

        /// <summary>
        /// Предупреждать об открытии каталога.
        /// </summary>
        bool WarningCatalog { get; }
    }
}