using System;
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

        /// <summary>
        /// Событие по изменению.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Событие.</param>
        public void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AppHelpers.DispatchAction(DelayAction);
        }

        private bool isWaiting;

        private bool isRefreshed;

        private DateTime stamp;

        private async void DelayAction()
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

        private void OnSizeUpdated()
        {
            SizeUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}