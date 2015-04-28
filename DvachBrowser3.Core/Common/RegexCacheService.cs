using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис кэширования регулярных выражений.
    /// </summary>
    public sealed class RegexCacheService : ServiceBase, IRegexCacheService
    {
        private static readonly Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public RegexCacheService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Создать регулярное выражение.
        /// </summary>
        /// <param name="expression">Выражение.</param>
        /// <returns>Объект.</returns>
        public Regex CreateRegex(string expression)
        {
            lock (RegexCache)
            {
                if (!RegexCache.ContainsKey(expression))
                {
                    RegexCache[expression] = new Regex(expression, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                }
                return RegexCache[expression];
            }
        }
    }
}