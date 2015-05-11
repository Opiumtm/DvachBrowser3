using System.Threading;

namespace DvachBrowser3
{
    /// <summary>
    /// Источник токена отмены.
    /// </summary>
    public interface ICancellationTokenSource
    {
        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        CancellationToken GetToken();
    }
}