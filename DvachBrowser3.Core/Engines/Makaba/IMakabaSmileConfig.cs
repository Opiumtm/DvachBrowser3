namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Конфигурация смайл-разметки.
    /// </summary>
    public interface IMakabaSmileConfig
    {
        /// <summary>
        /// Полужирный.
        /// </summary>
        string Bold { get; set; }

        /// <summary>
        /// Наклонный.
        /// </summary>
        string Italic { get; set; }

        /// <summary>
        /// Спойлер.
        /// </summary>
        string Spoiler { get; set; }

        /// <summary>
        /// Зачёркнутый.
        /// </summary>
        string Strike { get; set; }

        /// <summary>
        /// Моноширинный.
        /// </summary>
        string Monospace { get; set; }

        /// <summary>
        /// Подчёркнутый сверху.
        /// </summary>
        string Over { get; set; }

        /// <summary>
        /// Подчёркнутый сверху.
        /// </summary>
        string Under { get; set; }

        /// <summary>
        /// Верхний индекс.
        /// </summary>
        string Sup { get; set; }

        /// <summary>
        /// Нижний индекс.
        /// </summary>
        string Sub { get; set; }         
    }
}