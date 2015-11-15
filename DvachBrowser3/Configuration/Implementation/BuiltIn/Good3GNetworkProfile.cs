namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Хороший 3G.
    /// </summary>
    public sealed class Good3GNetworkProfile : INetworkProfile
    {
        /// <summary>
        /// Стандартный.
        /// </summary>
        public bool IsStandard => true;

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id => "Builtin.Good3G";

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => "Хороший 3G/4G";

        /// <summary>
        /// Обновлять страницу борды при входе.
        /// </summary>
        public bool UpdateBoardPageOnEntry => true;

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
        public bool AutoUpdateThreadAfterPost => true;

        /// <summary>
        /// Обновлять страницу треда при входе.
        /// </summary>
        public bool UpdateThreadPageOnEntry => true;

        /// <summary>
        /// Автоматически загружать предварительный просмотр изображений.
        /// </summary>
        public bool AutoLoadImageThumbnails => true;

        /// <summary>
        /// Проверять тред на апдейты (интервал проверки в секундах, null - не проверять).
        /// </summary>
        public double? CheckThreadForUpdatesSec => 60;

        /// <summary>
        /// Проверять на обновления вместо загрузки.
        /// </summary>
        public bool CheckForUpdatesInsteadOfLoad => false;

        /// <summary>
        /// Предпочитать частичную загрузку.
        /// </summary>
        public bool PreferPartialLoad => true;
    }
}