using Windows.Storage;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация отображения.
    /// </summary>
    public sealed class ViewConfiguration : AppDataConfigBase, IViewConfiguration
    {
        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.RoamingSettings.Values.ContainsKey("CommonView")
                ? (ApplicationDataCompositeValue)ApplicationData.Current.RoamingSettings.Values["CommonView"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.RoamingSettings.Values["CommonView"] = value;
        }

        /// <summary>
        /// Отображать даты в формате борды.
        /// </summary>
        public bool BoardNativeDates
        {
            get { return GetValue("BoardNativeDates", false); }
            set
            {
                SetValue("BoardNativeDates", value);
                OnPropertyChanged();
            }
        }
    }
}