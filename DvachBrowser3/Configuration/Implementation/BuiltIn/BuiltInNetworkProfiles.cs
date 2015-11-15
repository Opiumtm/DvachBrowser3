using System.Collections.Generic;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Встроенные профили сети.
    /// </summary>
    public class BuiltInNetworkProfiles
    {
        /// <summary>
        /// По умолчанию для мобильных.
        /// </summary>
        public static readonly INetworkProfile DefaultMobile = new DefaultMobileNetworkProfile();

        /// <summary>
        /// По умолчанию для десктопа.
        /// </summary>
        public static readonly INetworkProfile DefaultDesktop = new DefaultDesktopNetworkProfile();

        /// <summary>
        /// WiFi.
        /// </summary>
        public static readonly INetworkProfile Wifi = new WifiNetworkProfile();

        /// <summary>
        /// Хороший 3G.
        /// </summary>
        public static readonly INetworkProfile Good3G = new Good3GNetworkProfile();

        /// <summary>
        /// Плохой 3G.
        /// </summary>
        public static readonly INetworkProfile Bad3G = new Bad3GNetworkProfile();

        /// <summary>
        /// Минимальный.
        /// </summary>
        public static readonly INetworkProfile Minimal = new MinimalNetworkProfile();

        /// <summary>
        /// Профили.
        /// </summary>
        public static readonly Dictionary<string, INetworkProfile> Profiles = new Dictionary<string, INetworkProfile>()
        {
            { DefaultMobile.Id, DefaultMobile },
            { DefaultDesktop.Id, DefaultDesktop },
            { Wifi.Id, Wifi },
            { Good3G.Id, Good3G },
            { Bad3G.Id, Bad3G },
            { Minimal.Id, Minimal },
        };
    }
}