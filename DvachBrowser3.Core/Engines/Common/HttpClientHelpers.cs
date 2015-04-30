using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Классы помощники HTTP клиента.
    /// </summary>
    public static class HttpClientHelpers
    {
        /// <summary>
        /// Подсчитывать чтение из потока HTTP.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <param name="progress">Прогресс.</param>
        /// <returns>Поток с подсчётом.</returns>
        public static IInputStream AsHttpCounting(this IInputStream str, IProgress<ulong> progress)
        {
            return str;
        }
    }
}