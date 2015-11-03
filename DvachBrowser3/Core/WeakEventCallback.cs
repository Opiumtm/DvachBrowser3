using System;
using System.Threading;
using Template10.Common;

namespace DvachBrowser3
{
    /// <summary>
    /// Обратный вызов слабо связанного события.
    /// </summary>
    public sealed class WeakEventCallback : IWeakEventCallback, IDisposable
    {
        private readonly Guid token;

        private readonly WeakReference<IWeakEventChannel> channel;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="channel">Канал.</param>
        public WeakEventCallback(IWeakEventChannel channel)
        {
            this.channel = new WeakReference<IWeakEventChannel>(channel);
            token = channel.AddCallback(this);
        }

        /// <summary>
        /// Слабо связанное событие получено.
        /// </summary>
        public event WeakEventHandler WeakEventReceived;

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        public async void ReceiveWeakEvent(object sender, object e)
        {
            await BootStrapper.Current.NavigationService.Dispatcher.DispatchAsync(() =>
            {
                try
                {
                    WeakEventReceived?.Invoke(sender, new WeakEventArgs(e));
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            });
        }

        private int isDisposed = 0;

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref isDisposed, 1) == 0)
            {
                IWeakEventChannel channelObj;
                if (channel.TryGetTarget(out channelObj))
                {
                    channelObj.RemoveCallback(token);
                }
            }
        }
    }
}