namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Плохой 3G.
    /// </summary>
    public sealed class Bad3GNetworkProfile : INetworkProfile
    {
        /// <summary>
        /// Стандартный.
        /// </summary>
        public bool IsStandard => true;

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id => "Builtin.Bad3G";

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => "Плохой 3G/Edge";

        /// <summary>
        /// Обновлять страницу борды при входе.
        /// </summary>
        public bool UpdateBoardPageOnEntry => false;

        /// <summary>
        /// Показывать баннер.
        /// </summary>
        public bool ShowBanner => false;

        /// <summary>
        /// Проверять избранное на изменения.
        /// </summary>
        public bool CheckFavoritesForUpdates => true;

        /// <summary>
        /// Авоматически обновлять после поста.
        /// </summary>
        public bool AutoUpdateThreadAfterPost => false;

        /// <summary>
        /// Обновлять страницу треда при входе.
        /// </summary>
        public bool UpdateThreadPageOnEntry => false;

        /// <summary>
        /// Автоматически загружать предварительный просмотр изображений.
        /// </summary>
        public bool AutoLoadImageThumbnails => true;

        /// <summary>
        /// Проверять тред на апдейты (интервал проверки в секундах, null - не проверять).
        /// </summary>
        public double? CheckThreadForUpdatesSec => null;

        /// <summary>
        /// Проверять на обновления вместо загрузки.
        /// </summary>
        public bool CheckForUpdatesInsteadOfLoad => true;

        /// <summary>
        /// Предпочитать частичную загрузку.
        /// </summary>
        public bool PreferPartialLoad => true;

        /// <summary>
        /// Предупреждать о полной перезагрузке.
        /// </summary>
        public bool WarningFullReload => true;

        /// <summary>
        /// Предупреждать об открытии каталога.
        /// </summary>
        public bool WarningCatalog => true;
    }
}