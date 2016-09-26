namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Диагностика памяти.
    /// </summary>
    public interface IDiagnosticMemory
    {
        /// <summary>
        /// Общий объём памяти.
        /// </summary>
        IDiagnosticValue TotalCommitSize { get; }

        /// <summary>
        /// Приватно занятой памяти.
        /// </summary>
        IDiagnosticValue PrivateWorkingSet { get; }

        /// <summary>
        /// Занято управляемой памятью.
        /// </summary>
        IDiagnosticValue ManagedMemorySet { get; }

        /// <summary>
        /// Собрать мусор.
        /// </summary>
        void GcCollect();
    }
}