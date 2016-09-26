namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Информация о слабо связанных событиях.
    /// </summary>
    public interface IDiagnosticWeakEvents
    {
        /// <summary>
        /// Последний запуск очистки.
        /// </summary>
        IDiagnosticValue LastCleanupRun { get; }

        /// <summary>
        /// Активно словарей.
        /// </summary>
        IDiagnosticValue ActiveDictionaries { get; }

        /// <summary>
        /// Активно обратных вызовов.
        /// </summary>
        IDiagnosticValue ActiveCallbacks { get; }

        /// <summary>
        /// Очистить.
        /// </summary>
        void Cleanup();
    }
}