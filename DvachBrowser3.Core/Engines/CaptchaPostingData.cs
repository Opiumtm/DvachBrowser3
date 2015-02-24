namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Информация о капче.
    /// </summary>
    public abstract class CaptchaPostingData
    {
        /// <summary>
        /// Введённые данные капчи.
        /// </summary>
        public string CaptchaEntry { get; set; }
    }
}