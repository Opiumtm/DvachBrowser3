namespace DvachBrowser3
{
    /// <summary>
    /// Профиль использования сети.
    /// </summary>
    public enum NetworkProfile : int
    {
        /// <summary>
        /// Экстремальный режим.
        /// </summary>
        Extremal = 4,

        /// <summary>
        /// Подключение по WiFi.
        /// </summary>
        WiFi = 3,

        /// <summary>
        /// Хороший 3G.
        /// </summary>
        Good3G = 2,

        /// <summary>
        /// Плохой 3G.
        /// </summary>
        Bad3G = 1,

        /// <summary>
        /// Экономия трафика.
        /// </summary>
        Economic = 0
    }
}