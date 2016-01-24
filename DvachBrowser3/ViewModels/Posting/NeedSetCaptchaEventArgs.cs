using System;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргумент события запроса на капчу.
    /// </summary>
    public sealed class NeedSetCaptchaEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="captchaType">Тип капчи.</param>
        /// <param name="engine">Движок.</param>
        public NeedSetCaptchaEventArgs(CaptchaType captchaType, string engine)
        {
            CaptchaType = captchaType;
            Engine = engine;
        }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        public CaptchaType CaptchaType { get; private set; }

        /// <summary>
        /// Движок.
        /// </summary>
        public string Engine { get; private set; }
    }

    /// <summary>
    /// Обработчик события по запросу на капчу.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Событие.</param>
    public delegate void NeedSetCaptchaEventHandler(object sender, NeedSetCaptchaEventArgs e);
}