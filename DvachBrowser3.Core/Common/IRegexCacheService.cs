using System.Text.RegularExpressions;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис кэширования регулярных выражений.
    /// </summary>
    public interface IRegexCacheService
    {
        /// <summary>
        /// Создать регулярное выражение.
        /// </summary>
        /// <param name="expression">Выражение.</param>
        /// <returns>Объект.</returns>
        Regex CreateRegex(string expression);
    }
}