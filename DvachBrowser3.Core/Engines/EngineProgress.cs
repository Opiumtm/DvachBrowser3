using System;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Прогресс сетевой связи.
    /// </summary>
    public class EngineProgress : EventArgs
    {
        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Прогресс в процентах.
        /// </summary>
        public double? Percent { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="percent">Прогресс в процентах.</param>
        public EngineProgress(string message, double? percent = null)
        {
            Message = message;
            Percent = percent;
        }
    }
}