using System;
using Windows.Storage;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация профиля сети.
    /// </summary>
    public sealed class NetworkProfileConfiguration : AppDataConfigBase, INetworkProfileConfiguration
    {
        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.RoamingSettings.Values.ContainsKey("NetworkProfile")
                ? (ApplicationDataCompositeValue)ApplicationData.Current.RoamingSettings.Values["NetworkProfile"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.RoamingSettings.Values["NetworkProfile"] = value;
        }

        /// <summary>
        /// Частичная загрузка треда.
        /// </summary>
        public bool PartialThreadLoad
        {
            get { return GetValue("PartialThreadLoad", true); }
            set
            {
                SetValue("PartialThreadLoad", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Загружать тред на старте.
        /// </summary>
        public LoadThreadOnStartMode LoadThreadOnStart
        {
            get { return (LoadThreadOnStartMode)GetValue("LoadThreadOnStart", (int)(LoadThreadOnStartMode.Load)); }
            set
            {
                SetValue("LoadThreadOnStart", (int)value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Проверка тредов на обновления.
        /// </summary>
        public TimeSpan UpdateCheckPeriod
        {
            get { return TimeSpan.FromSeconds(GetValue("UpdateCheckPeriod", 30.0)); }
            set
            {
                SetValue("UpdateCheckPeriod", value.TotalSeconds);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Разрешить проверку тредов на обновления.
        /// </summary>
        public bool UpdateCheck
        {
            get { return GetValue("UpdateCheck", true); }
            set
            {
                SetValue("UpdateCheck", value);
                OnPropertyChanged();
            }
        }
    }
}