namespace DvachBrowser3.System
{
    /// <summary>
    /// Информация о системе.
    /// </summary>
    public interface ISystemInfo
    {
        /// <summary>
        /// Платформа.
        /// </summary>
        AppPlatform Platform { get; } 
    }
}