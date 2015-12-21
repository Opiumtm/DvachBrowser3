using System.Threading;

namespace DvachBrowser3
{
    /// <summary>
    /// Контейнер флага.
    /// </summary>
    public sealed class InterlockedFlagContainer
    {
        /// <summary>
        /// Данные флага.
        /// </summary>
        public int FlagData;

        /// <summary>
        /// Флаг.
        /// </summary>
        public bool Flag
        {
            get { return Interlocked.CompareExchange(ref FlagData, 0, 0) != 0; }
            set { Interlocked.Exchange(ref FlagData, value ? 1 : 0); }
        }
    }
}