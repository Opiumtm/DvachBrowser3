using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Engines.Makaba;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Информация о движках.
    /// </summary>
    public sealed class NetworkEngines : ServiceBase, INetworkEngines
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NetworkEngines(IServiceProvider services) : base(services)
        {
            engines = new Dictionary<string, INetworkEngine>(StringComparer.OrdinalIgnoreCase)
            {
                { CoreConstants.Engine.Makaba, new MakabaEngine(Services) }
            };
        }

        private readonly Dictionary<string, INetworkEngine> engines;

        /// <summary>
        /// Перечислить движки.
        /// </summary>
        /// <returns>Движки.</returns>
        public IReadOnlyCollection<string> ListEngines()
        {
            return engines.Keys.ToArray();
        }

        /// <summary>
        /// Получить движок по его идентификатору.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Идентификатор.</returns>
        public INetworkEngine GetEngineById(string engine)
        {
            if (engines.ContainsKey(engine))
            {
                return engines[engine];
            }
            throw new InvalidOperationException(string.Format("Движок c идентификатором \"{0}\" не найден", engine));
        }
    }
}