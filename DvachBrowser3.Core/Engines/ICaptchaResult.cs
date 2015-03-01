using Windows.Storage;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат капчи.
    /// </summary>
    public interface ICaptchaResult
    {
        /// <summary>
        /// Нужна капча.
        /// </summary>
        bool NeedCaptcha { get; }

        /// <summary>
        /// Ключи.
        /// </summary>
        CaptchaKeys Keys { get; }
    }
}