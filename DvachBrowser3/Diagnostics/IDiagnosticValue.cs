using System;
using System.Threading.Tasks;

namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Диагностическое значение.
    /// </summary>
    public interface IDiagnosticValue
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Запросить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        Task<string> QueryAsync();

        /// <summary>
        /// Значение изменилось.
        /// </summary>
        event EventHandler ValueChanged;
    }
}