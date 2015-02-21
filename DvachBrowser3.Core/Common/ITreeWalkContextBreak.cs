namespace DvachBrowser3
{
    /// <summary>
    /// Возможность прервать обработку.
    /// </summary>
    public interface ITreeWalkContextBreak
    {
        /// <summary>
        /// Прервать обработку.
        /// </summary>
        bool IsBreak { get; set; }
    }
}