namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат проверки возможности постинга без капчи.
    /// </summary>
    public interface INoCaptchaCheckResult
    {
        /// <summary>
        /// Можно постить без капчи.
        /// </summary>
        bool CanPost { get; }         

        /// <summary>
        /// Возможность.
        /// </summary>
        NoCaptchaAbility Ability { get; }
    }

    /// <summary>
    /// Возможность постинга без капчи.
    /// </summary>
    public enum NoCaptchaAbility
    {
        /// <summary>
        /// Можно постить.
        /// </summary>
        Ok,

        /// <summary>
        /// Нет ключей API.
        /// </summary>
        NoApiKeys,

        /// <summary>
        /// Неправильные ключи.
        /// </summary>
        InvalidKeys,

        /// <summary>
        /// Запрещено для данной ссылки.
        /// </summary>
        ForbiddenForLink,

        /// <summary>
        /// Попробуйте позже.
        /// </summary>
        TryAgain,
    }
}