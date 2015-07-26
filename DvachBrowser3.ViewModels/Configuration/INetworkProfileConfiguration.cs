using System;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация профиля сети.
    /// </summary>
    public interface INetworkProfileConfiguration
    {
        /// <summary>
        /// Частичная загрузка треда.
        /// </summary>
        bool PartialThreadLoad { get; set; }

        /// <summary>
        /// Загружать тред на старте.
        /// </summary>
        LoadThreadOnStartMode LoadThreadOnStart { get; set; }

        /// <summary>
        /// Проверка тредов на обновления.
        /// </summary>
        TimeSpan UpdateCheckPeriod { get; set; }

        /// <summary>
        /// Разрешить проверку тредов на обновления.
        /// </summary>
        bool UpdateCheck { get; set; }
    }
}