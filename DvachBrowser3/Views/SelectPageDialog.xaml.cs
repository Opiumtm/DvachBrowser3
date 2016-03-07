using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class SelectPageDialog : ContentDialog, INotifyPropertyChanged
    {
        public SelectPageDialog()
        {
            this.InitializeComponent();
            this.DataContext = this;
            ValidateEntry();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int v;
            if (!int.TryParse(PageBox.Text.Trim(), out v))
            {
                args.Cancel = true;
                return;
            }
            if (v > MaxPage || v < MinPage)
            {
                args.Cancel = true;
                return;
            }
            SelectedPage = v;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        /// <summary>
        /// Выбранная страница.
        /// </summary>
        public int? SelectedPage { get; private set; }

        private int minPage;

        /// <summary>
        /// Минимальная страница.
        /// </summary>
        public int MinPage
        {
            get { return minPage; }
            set
            {
                minPage = value;
                OnPropertyChanged();
                ValidateEntry();
            }
        }

        private int maxPage;

        /// <summary>
        /// Максимальная страница.
        /// </summary>
        public int MaxPage
        {
            get { return maxPage; }
            set
            {
                maxPage = value;
                OnPropertyChanged();
                ValidateEntry();
            }
        }

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PageBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            int v;
            if (!int.TryParse(PageBox.Text.Trim(), out v))
            {
                IsPrimaryButtonEnabled = false;
                return;
            }
            if (v > MaxPage || v < MinPage)
            {
                IsPrimaryButtonEnabled = false;
                return;
            }
            IsPrimaryButtonEnabled = true;
        }
    }
}
