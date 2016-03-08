using System;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник в задержке события.
    /// </summary>
    public sealed class EventDelayHelper
    {
        private readonly TimeSpan eventDelay;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="eventDelay">Задержка вызова события.</param>
        public EventDelayHelper(TimeSpan eventDelay)
        {
            this.eventDelay = eventDelay;
        }

        /// <summary>
        /// Вызвать событие.
        /// </summary>
        public void Trigger()
        {
            AppHelpers.DispatchAction(DelayAction, false, 0);
        }

        /// <summary>
        /// Событие вызвано.
        /// </summary>
        public event EventHandler EventFired;

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
                    var toWait = (stamp.Add(eventDelay) - DateTime.Now);
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
            AppHelpers.DispatchAction(FireEvent, false, 0);
        }

        private Task FireEvent()
        {
            EventFired?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }
    }
}