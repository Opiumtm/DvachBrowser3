//#define LOG_ERROR_DEBUG

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using DvachBrowser3.Engines;
using DvachBrowser3.Views;
using Template10.Common;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощники приложения.
    /// </summary>
    public static class AppHelpers
    {
        /// <summary>
        /// Показать ошибку на экране.
        /// </summary>
        /// <param name="ex">Ошибка.</param>
        /// <returns>Результат.</returns>
        public static async Task ShowError(Exception ex)
        {
#if DEBUG
#if LOG_ERROR_DEBUG
            try
            {
                if (ex != null)
                {
                    var fld = await ApplicationData.Current.LocalFolder.CreateFolderAsync("error_log", CreationCollisionOption.OpenIfExists);
                    var f = await fld.CreateFileAsync(Guid.NewGuid() + ".error");
                    using (var str = await f.OpenStreamForWriteAsync())
                    {
                        using (var wr = new StreamWriter(str, Encoding.UTF8))
                        {
                            await wr.WriteAsync(ex.ToString());
                            await wr.FlushAsync();
                        }
                    }
                }
            }
            catch
            {
            }
#endif
#endif
            var dialog = new MessageDialog(ex?.Message ?? "", AppConstants.ErrorTitle);
            dialog.Commands.Add(new UICommand(AppConstants.OkButtonLabel, command => { }));
            dialog.Commands.Add(new UICommand(AppConstants.MoreInfoButtonLabel, async command =>
            {
                await ShowFullError(ex);
            }));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Показать ошибку на экране.
        /// </summary>
        /// <param name="ex">Ошибка.</param>
        /// <returns>Результат.</returns>
        public static async Task ShowFullError(Exception ex)
        {
            var dialog = new ErrorInfoDialog();
            dialog.Error = ex?.ToString() ?? "";
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Найти сетевой движок.
        /// </summary>
        /// <param name="engines">Движки.</param>
        /// <param name="id">Идентификатор движка.</param>
        /// <returns>Результат.</returns>
        public static INetworkEngine FindEngine(this INetworkEngines engines, string id)
        {
            if (engines == null) throw new ArgumentNullException(nameof(engines));
            if (id == null)
            {
                return null;
            }
            if (!engines.ListEngines().Any(e => StringComparer.OrdinalIgnoreCase.Equals(e, id)))
            {
                return null;
            }
            return engines.GetEngineById(id);
        }

        /// <summary>
        /// Диспетчер.
        /// </summary>
        public static IDispatcherWrapper Dispatcher => BootStrapper.Current?.NavigationService?.Dispatcher;

        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<bool> isMobile = new Lazy<bool>(() => Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile");

        /// <summary>
        /// Мобильное приложение.
        /// </summary>
        public static bool IsMobile => isMobile.Value;

        /// <summary>
        /// Выполнить дейсвтие на диспетчере.
        /// </summary>
        /// <param name="a">Действие.</param>
        /// <param name="reportError">Показать ошибку.</param>
        public static void DispatchAction(Func<Task> a, bool reportError = false)
        {
            if (a == null)
            {
                return;
            }
            var disp = Dispatcher;
            disp?.DispatchAsync(async () =>
            {
                await RunAction(a, reportError);
            });
        }

        private static async Task RunAction(Func<Task> a, bool reportError = false)
        {
            if (a == null)
            {
                return;
            }
            try
            {
                await a();
            }
            catch (Exception ex)
            {
                if (reportError)
                {
                    await ShowError(ex);
                }
                else
                {
                    DebugHelper.BreakOnError(ex);
                }
            }
        }

        /// <summary>
        /// Выполнить дейсвтие на UI треде.
        /// </summary>
        /// <param name="a">Действие.</param>
        /// <param name="reportError">Показать ошибку.</param>
        public static async void ActionOnUiThread(Func<Task> a, bool reportError = false)
        {
            if (a == null)
            {
                return;
            }
            if (OnUiThread())
            {
                await RunAction(a, reportError);
            }
            else
            {
                DispatchAction(a, reportError);
            }
        }

        /// <summary>
        /// Запущено на UI-потоке.
        /// </summary>
        /// <returns>Результат.</returns>
        public static bool OnUiThread()
        {
            var disp = Dispatcher;
            return disp?.HasThreadAccess() ?? false;
        }
    }
}