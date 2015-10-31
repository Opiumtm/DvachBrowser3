using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Класс-помощник фильрации борд.
    /// </summary>
    public static class BoardListBoardViewModelsHelper
    {
        /// <summary>
        /// Имена движков.
        /// </summary>
        private static readonly Dictionary<string, string> EngineNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Имена движков.
        /// </summary>
        private static readonly Dictionary<string, string> ResourceNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Фильтровать.
        /// </summary>
        /// <param name="board">Борда.</param>
        /// <param name="filter">Фильтр.</param>
        /// <returns>Результат.</returns>
        public static bool Filter(IBoardListBoardViewModel board, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }
            filter = filter.Trim().ToLower();
            return (board?.ShortName ?? "").ToLower().Contains(filter) || (board?.DisplayName ?? "").ToLower().Contains(filter);
        }

        /// <summary>
        /// Получить имя движка.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Имя движка.</returns>
        public static string GetEngineName(string engine)
        {
            EnsureEngine(engine ?? "");
            return EngineNames[engine ?? ""];
        }

        /// <summary>
        /// Получить имя ресурса.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Имя ресурса.</returns>
        public static string GetResourceName(string engine)
        {
            EnsureEngine(engine ?? "");
            return ResourceNames[engine ?? ""];
        }

        private static void EnsureEngine(string engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            if (!EngineNames.ContainsKey(engine) || !ResourceNames.ContainsKey(engine))
            {
                var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                if (engines.ListEngines().Any(e => engine.Equals(e, StringComparison.OrdinalIgnoreCase)))
                {
                    var engineObj = engines.GetEngineById(engine);
                    EngineNames[engine] = engineObj.DisplayName;
                    ResourceNames[engine] = engineObj.ResourceName;
                }
                else
                {
                    EngineNames[engine] = "Неизвестный движок";
                    ResourceNames[engine] = "Неизвестный ресурс";
                }
            }
        }
    }
}