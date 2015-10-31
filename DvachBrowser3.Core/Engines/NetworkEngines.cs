using System;
using System.Collections.Generic;
using System.Linq;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Информация о движках.
    /// </summary>
    public sealed class NetworkEngines : ServiceBase, INetworkEngines, INetworkEngineInstaller
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NetworkEngines(IServiceProvider services) : base(services)
        {
            engines = new Dictionary<string, INetworkEngine>(StringComparer.OrdinalIgnoreCase);
        }

        private readonly Dictionary<string, INetworkEngine> engines;

        /// <summary>
        /// Перечислить движки.
        /// </summary>
        /// <returns>Движки.</returns>
        public IReadOnlyCollection<string> ListEngines()
        {
            lock (engines)
            {
                return engines.Keys.ToArray();
            }
        }

        /// <summary>
        /// Получить движок по его идентификатору.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Идентификатор.</returns>
        public INetworkEngine GetEngineById(string engine)
        {
            lock (engines)
            {
                if (engines.ContainsKey(engine))
                {
                    return engines[engine];
                }
            }
            throw new InvalidOperationException(string.Format("Движок c идентификатором \"{0}\" не найден", engine));
        }

        /// <summary>
        /// Установить сетевой движок.
        /// </summary>
        /// <param name="engine">Сетевой движок.</param>
        public void Install(INetworkEngine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            lock (engines)
            {
                engines[engine.EngineId] = engine;
            }
        }
    }
}