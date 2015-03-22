namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат проверки изменения ресурса.
    /// </summary>
    public interface ILastModifiedCheckResult
    {
        /// <summary>
        /// Результат.
        /// </summary>
        string LastModified { get; } 
    }
}