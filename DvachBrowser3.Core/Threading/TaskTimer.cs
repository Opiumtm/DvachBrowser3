using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3
{
    /// <summary>
    /// Таймер.
    /// </summary>
    public sealed class TaskTimer
    {
        /// <summary>
        /// Действие по пройденному промежутку времени.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        public TaskTimer(CoreDispatcher dispatcher = null)
        {
            this.dispatcher = dispatcher ?? CoreWindow.GetForCurrentThread()?.Dispatcher;
        }

        /// <summary>
        /// Период таймера в секундах.
        /// </summary>
        private long period = TimeSpan.FromSeconds(1).Ticks;

        public TimeSpan Period
        {
            get
            {
                var ticks = Interlocked.CompareExchange(ref period, 0, 0);
                return new TimeSpan(ticks);
            }
            set
            {
                var ticks = value.Ticks;
                Interlocked.Exchange(ref period, ticks);
            }
        }

        private int isEnabled = 0;

       public bool IsEnabled
        {
            get { return Interlocked.CompareExchange(ref isEnabled, 0, 0) != 0; }
            set
            {
                var oldValue = Interlocked.Exchange(ref isEnabled, value ? 1 : 0);
                if (oldValue == 0 && value && dispatcher != null)
                {
                    StartTimer();
                }
            }
        }

        private readonly CoreDispatcher dispatcher;

        private readonly Random random = new Random();

        private void StartTimer()
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (!dispatcher.HasThreadAccess)
                {
                    dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TimerMethod();
                    });
                }
                else
                {
                    currentHandler = random.Next();
                    TimerMethod();
                }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private int currentHandler;

        private async Task TimerMethod()
        {
            var thisHandler = random.Next();
            currentHandler = thisHandler;
            while (IsEnabled && currentHandler == thisHandler)
            {
                try
                {
                    var waitPeriod = Period;
                    if (waitPeriod < TimeSpan.FromMilliseconds(10))
                    {
                        waitPeriod = TimeSpan.FromMilliseconds(10);
                    }
                    await Task.Delay(waitPeriod);
                    if (IsEnabled && currentHandler == thisHandler)
                    {
                        Tick?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
        }
    }
}