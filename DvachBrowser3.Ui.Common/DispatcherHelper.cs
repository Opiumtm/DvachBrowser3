using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui
{
    /// <summary>
    /// Помощник в диспетчеризации.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Выполнить действие на диспетчере.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public static async Task DispatchAction(this CoreDispatcher dispatcher, Action action)
        {
            if (dispatcher == null || dispatcher.HasThreadAccess)
            {
                action?.Invoke();
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    action();
                });
            }
        }

        /// <summary>
        /// Выполнить действие на диспетчере.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public static async Task<T> DispatchFunc<T>(this CoreDispatcher dispatcher, Func<T> action)
        {
            if (action == null)
            {
                return default(T);
            }
            if (dispatcher == null || dispatcher.HasThreadAccess)
            {
                return action();
            }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        var r = action();
                        tcs.SetResult(r);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
                return await tcs.Task;
            }
        }

        /// <summary>
        /// Выполнить действие на диспетчере.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public static async Task DispatchTask(this CoreDispatcher dispatcher, Func<Task> action)
        {
            if (action == null)
            {
                return;
            }
            if (dispatcher == null || dispatcher.HasThreadAccess)
            {
                await action();
            }
            else
            {
                var tcs = new TaskCompletionSource<int>();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        await action();
                        tcs.SetResult(0);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
                await tcs.Task;
            }
        }

        /// <summary>
        /// Выполнить действие на диспетчере.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        public static async Task<T> DispatchTask<T>(this CoreDispatcher dispatcher, Func<Task<T>> action)
        {
            if (action == null)
            {
                return default(T);
            }
            if (dispatcher == null || dispatcher.HasThreadAccess)
            {
                return await action();
            }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        var r = await action();
                        tcs.SetResult(r);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
                return await tcs.Task;
            }
        }
    }
}