namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Инсталлятор сетевых движков.
    /// </summary>
    public interface INetworkEngineInstaller
    {
        /// <summary>
        /// Установить сетевой движок.
        /// </summary>
        /// <param name="engine">Сетевой движок.</param>
        void Install(INetworkEngine engine);
    }
}