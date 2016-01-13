using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощник задержки изменения размеров.
    /// </summary>
    public sealed class SizeChangeDelayHelper
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sizeChangeDelay">Задержка.</param>
        public SizeChangeDelayHelper(TimeSpan sizeChangeDelay)
        {
            this.sizeChangeDelay = sizeChangeDelay;
        }

        /// <summary>
        /// Размер изменён.
        /// </summary>
        public event EventHandler SizeUpdated;

        private readonly TimeSpan sizeChangeDelay;

        private int isFirst;

        /// <summary>
        /// Событие по изменению.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Событие.</param>
        public void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Interlocked.Exchange(ref isFirst, 1) == 0)
            {
                AppHelpers.DispatchAction(OnSizeUpdated);
                return;
            }
            AppHelpers.DispatchAction(DelayAction);
        }

        private bool isWaiting;

        private bool isRefreshed;

        private DateTime stamp;

        private async Task DelayAction()
        {
            stamp = DateTime.Now;
            isRefreshed = true;
            if (isWaiting)
            {
                return;
            }
            isWaiting = true;
            try
            {
                while (isRefreshed)
                {
                    isRefreshed = false;
                    var toWait = (stamp.Add(sizeChangeDelay) - DateTime.Now);
                    if (toWait.Ticks > 0)
                    {
                        await Task.Delay(toWait);
                    }
                }
            }
            finally
            {
                isWaiting = false;
            }
            OnSizeUpdated();
        }

        private Task OnSizeUpdated()
        {
            SizeUpdated?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }
    }
}