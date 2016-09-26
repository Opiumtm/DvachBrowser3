using System;
using System.Threading.Tasks;

namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Диагностическое значение.
    /// </summary>
    public abstract class DiagnosticValueBase : IDiagnosticValue
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Запросить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        public abstract Task<string> QueryAsync();

        /// <summary>
        /// Значение изменилось.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Вызвать изменение.
        /// </summary>
        public void TriggerChange()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}