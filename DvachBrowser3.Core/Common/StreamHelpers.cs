using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Buffer = Windows.Storage.Streams.Buffer;

namespace DvachBrowser3
{
    /// <summary>
    /// Классы помощники.
    /// </summary>
    public static class StreamHelpers
    {
        /// <summary>
        /// Копировать из RT потока в .NET поток с прогрессом.
        /// </summary>
        /// <param name="src">Исхоный поток.</param>
        /// <param name="outStream">Поток результата.</param>
        /// <param name="progress">Прогресс.</param>
        /// <param name="token">Токен отмены.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Таск.</returns>
        public static async Task CopyToNetStreamWithProgress(this IInputStream src, Stream outStream, IProgress<ulong> progress, CancellationToken token, uint bufferSize = 16384)
        {
            ulong totalRead = 0;
            using (var rd = new DataReader(src))
            {
                progress.Report(0);
                do {
                    token.ThrowIfCancellationRequested();
                    var r = await rd.LoadAsync(bufferSize);
                    totalRead += r;
                    progress.Report(totalRead);
                    if (r <= 0) break;
                    var buf = new byte[r];
                    rd.ReadBytes(buf);
                    await outStream.WriteAsync(buf, 0, (int)r, token);
                } while (true);
                rd.DetachStream();
            }
        }

        public static async Task CopyStreamAsync(this IInputStream src, IOutputStream outStream, uint bufferSize = 16384)
        {
            IBuffer buf2;
            do
            {
                var buf = new Buffer(bufferSize);
                buf2 = await src.ReadAsync(buf, bufferSize, InputStreamOptions.None);
                if (buf2.Length > 0)
                {
                    await outStream.WriteAsync(buf2);
                }
            } while (buf2.Length > 0);
        }
    }
}