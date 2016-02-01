using System;
using DvachBrowser3.Engines;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Результат запроса на капчу.
    /// </summary>
    public sealed class CaptchaQueryResultEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные капчи.</param>
        public CaptchaQueryResultEventArgs(CaptchaPostingData data)
        {
            Data = data;
        }

        /// <summary>
        /// Данные капчи.
        /// </summary>
        public CaptchaPostingData Data { get; private set; }         
    }

    /// <summary>
    /// Обработчик события по результату запроса на капчу.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Событие.</param>
    public delegate void CaptchaQueryResultEventHandler(object sender, CaptchaQueryResultEventArgs e);
}