namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Класс помощник по сетевым профилям.
    /// </summary>
    public static class NetworkProfileHelper
    {
        /// <summary>
        /// Текущий профиль.
        /// </summary>
        public static INetworkProfile CurrentProfile => ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>().CurrentProfile;
    }
}