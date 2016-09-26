namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Диагностические данные.
    /// </summary>
    public sealed class DiagnosticData : IDiagnosticData
    {
        /// <summary>
        /// Память.
        /// </summary>
        public IDiagnosticMemory Memory { get; } = new DiagnosticMemory();


        /// <summary>
        /// Слабо связанные события.
        /// </summary>
        public IDiagnosticWeakEvents WeakEvents { get; } = new DiagnosticWeakEvents();
    }
}