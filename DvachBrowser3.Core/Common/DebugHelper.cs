using System;
using System.Diagnostics;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс помощник для отладки.
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// Остановиться при ошибке.
        /// </summary>
        /// <param name="ex">Ошибка.</param>
        public static void BreakOnError(Exception ex)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
#endif
        }
    }
}