using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        /// Имена движков.
        /// </summary>
        private static readonly Dictionary<string, Color> EngineColors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Логотипы движков.
        /// </summary>
        private static readonly Dictionary<string, ImageSource> EngineLogos = new Dictionary<string, ImageSource>(StringComparer.OrdinalIgnoreCase);

        static BoardListBoardViewModelsHelper()
        {
            var makabaUri = new Uri("ms-appx:///Resources/MakabaLogo.png", UriKind.Absolute);
            EngineLogos[CoreConstants.Engine.Makaba] = new BitmapImage(makabaUri);
        }

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

        /// <summary>
        /// Получить цвет ресурса.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Цвет ресурса.</returns>
        public static Color GetResourceColor(string engine)
        {
            EnsureEngine(engine ?? "");
            return EngineColors[engine ?? ""];
        }

        /// <summary>
        /// ПОлучить логотип.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Логотип.</returns>
        public static ImageSource GetLogo(string engine)
        {
            return EngineLogos.ContainsKey(engine) ? EngineLogos[engine] : null;
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
                    EngineColors[engine] = engineObj.TileBackgroundColor;
                }
                else
                {
                    EngineNames[engine] = "Неизвестный движок";
                    ResourceNames[engine] = "Неизвестный ресурс";
                    EngineColors[engine] = Colors.DarkGray;
                }
            }
        }
    }
}