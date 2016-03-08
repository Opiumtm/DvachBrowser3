using System;
using System.Collections.Generic;

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
        /// Дополнительные данные.
        /// </summary>
        public EngineProgressOtherData OtherData { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="percent">Прогресс в процентах.</param>
        /// <param name="otherData">Дополнительные данные.</param>
        public EngineProgress(string message, double? percent, EngineProgressOtherData otherData = null)
        {
            Message = message;
            Percent = percent;
            OtherData = otherData;
        }
    }

    /// <summary>
    /// Дополнительные данные.
    /// </summary>
    public class EngineProgressOtherData
    {
        /// <summary>
        /// Тип операции.
        /// </summary>
        public string Kind { get; set; }        
    }
}