namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат запроса на последнее изменение.
    /// </summary>
    public interface ILastModifiedResult
    {
        /// <summary>
        /// Последнее изменение.
        /// </summary>
        string LastModified { get; }
    }
}