using System;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Ключи капчи.
    /// </summary>
    public abstract class CaptchaKeys
    {
        /// <summary>
        /// Тип капчи.
        /// </summary>
        public abstract CaptchaType Kind { get; }

        /// <summary>
        /// URI изображения капчи.
        /// </summary>
        public Uri CaptchaDisplayUri { get; set; }
    }
}