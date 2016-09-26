namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Диагностические данные.
    /// </summary>
    public interface IDiagnosticData
    {
        /// <summary>
        /// Память.
        /// </summary>
        IDiagnosticMemory Memory { get; }

        /// <summary>
        /// Слабо связанные события.
        /// </summary>
        IDiagnosticWeakEvents WeakEvents { get; }
    }
}