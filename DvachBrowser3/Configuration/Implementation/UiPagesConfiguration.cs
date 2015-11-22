using Windows.Storage;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация страниц UI.
    /// </summary>
    public class UiPagesConfiguration : AppDataConfigBase, IUiPagesConfiguration
    {
        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey("UiPages")
                ? (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["UiPages"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.LocalSettings.Values["UiPages"] = value;
        }

        private bool IsMobile => AppHelpers.IsMobile;

        /// <summary>
        /// Показывать баннеры.
        /// </summary>
        public bool ShowBanners
        {
            get { return GetValue("ShowBanners", !IsMobile); }
            set
            {
                SetValue("ShowBanners", value);
                OnPropertyChanged();
            }
        }
    }
}