using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// Подробная информация об ошибке.
    /// </summary>
    public sealed partial class ErrorInfoDialog : ContentDialog
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ErrorInfoDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Error
        {
            get { return ErrorText.Text; }
            set { ErrorText.Text = value; }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
