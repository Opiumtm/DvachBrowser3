using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис работы с датами.
    /// </summary>
    public interface IDateService
    {
        /// <summary>
        /// Перевести из UNIX-времени.
        /// </summary>
        /// <param name="timestamp">Временная метка.</param>
        /// <returns>Время.</returns>
        DateTime FromUnixTime(int timestamp);

        /// <summary>
        /// Время для пользователя.
        /// </summary>
        /// <param name="dateTime">Время.</param>
        /// <returns>Строка.</returns>
        string ToUserString(DateTime dateTime);         
    }
}