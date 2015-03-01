using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис работы с датами.
    /// </summary>
    public class DateService : ServiceBase, IDateService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public DateService(IServiceProvider services)
            : base(services)
        {
        }

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Перевести из UNIX-времени.
        /// </summary>
        /// <param name="timestamp">Временная метка.</param>
        /// <returns>Время.</returns>
        public DateTime FromUnixTime(int timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// Время для пользователя.
        /// </summary>
        /// <param name="dateTime">Время.</param>
        /// <returns>Строка.</returns>
        public string ToUserString(DateTime dateTime)
        {
            string dow = "";
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dow = "Пнд";
                    break;
                case DayOfWeek.Tuesday:
                    dow = "Втр";
                    break;
                case DayOfWeek.Wednesday:
                    dow = "Срд";
                    break;
                case DayOfWeek.Thursday:
                    dow = "Чтв";
                    break;
                case DayOfWeek.Friday:
                    dow = "Птн";
                    break;
                case DayOfWeek.Saturday:
                    dow = "Сбт";
                    break;
                case DayOfWeek.Sunday:
                    dow = "Вск";
                    break;
            }
            return string.Format("{5} {0:D2}.{1:D2}.{2:D4} {3:D2}:{4:D2}", dateTime.Day, dateTime.Month, dateTime.Year, dateTime.Hour, dateTime.Minute, dow);
        }         
    }
}