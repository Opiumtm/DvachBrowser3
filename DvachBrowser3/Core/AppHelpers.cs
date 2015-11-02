using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
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
    }
}