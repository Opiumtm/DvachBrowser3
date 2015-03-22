using Windows.Storage;
using DvachBrowser3.Engines.Makaba;

namespace DvachBrowser3.Configuration.Makaba
{
    /// <summary>
    /// Конфигурация постинга макабы.
    /// </summary>
    public class MakabaPostConfig : AppDataConfigBase, IMakabaPostConfig, IMakabaSmileConfig
    {
        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey("MakabaPost")
                ? (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["MakabaPost"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.LocalSettings.Values["MakabaPost"] = value;
        }

        /// <summary>
        /// Смайл-разметка.
        /// </summary>
        public IMakabaSmileConfig SmileConfig
        {
            get { return this; }
        }

        /// <summary>
        /// Корректировать разметку.
        /// </summary>
        public bool CorrectWakaba
        {
            get { return GetValue("CorrectWakaba", true); }
            set
            {
                SetValue("CorrectWakaba", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Использовать смайл-разметку.
        /// </summary>
        public bool UseSmileMarkup
        {
            get { return GetValue("UseSmileMarkup", true); }
            set
            {
                SetValue("UseSmileMarkup", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Полужирный.
        /// </summary>
        public string Bold
        {
            get { return GetValue("Smile.Bold", CharHelper.CharArrToString(128515)); }
            set
            {
                SetValue("Smile.Bold", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Наклонный.
        /// </summary>
        public string Italic
        {
            get { return GetValue("Smile.Italic", CharHelper.CharArrToString(128521)); }
            set
            {
                SetValue("Smile.Italic", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Спойлер.
        /// </summary>
        public string Spoiler
        {
            get { return GetValue("Smile.Spoiler", CharHelper.CharArrToString(128520)); }
            set
            {
                SetValue("Smile.Spoiler", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Зачёркнутый.
        /// </summary>
        public string Strike
        {
            get { return GetValue("Smile.Strike", CharHelper.CharArrToString(128528)); }
            set
            {
                SetValue("Smile.Strike", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Моноширинный.
        /// </summary>
        public string Monospace
        {
            get { return GetValue("Smile.Monospace", CharHelper.CharArrToString(128542)); }
            set
            {
                SetValue("Smile.Monospace", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Подчёркнутый сверху.
        /// </summary>
        public string Over
        {
            get { return GetValue("Smile.Over", CharHelper.CharArrToString(128513)); }
            set
            {
                SetValue("Smile.Over", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Подчёркнутый сверху.
        /// </summary>
        public string Under
        {
            get { return GetValue("Smile.Under", CharHelper.CharArrToString(128514)); }
            set
            {
                SetValue("Smile.Under", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Верхний индекс.
        /// </summary>
        public string Sup
        {
            get { return GetValue("Smile.Sup", CharHelper.CharArrToString(128526)); }
            set
            {
                SetValue("Smile.Sup", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Нижний индекс.
        /// </summary>
        public string Sub
        {
            get { return GetValue("Smile.Sub", CharHelper.CharArrToString(128525)); }
            set
            {
                SetValue("Smile.Sub", value);
                OnPropertyChanged();
            }
        }
    }
}