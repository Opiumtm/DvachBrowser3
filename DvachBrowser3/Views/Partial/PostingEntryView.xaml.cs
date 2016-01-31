using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostingEntryView : UserControl, IWeakEventCallback
    {
        public PostingEntryView()
        {
            this.InitializeComponent();
            Shell.IsNarrowViewChanged.AddCallback(this);
            AppHelpers.ActionOnUiThread(IsNarrowViewChanged);
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == Shell.IsNarrowViewChangedId)
            {
                AppHelpers.ActionOnUiThread(IsNarrowViewChanged);
            }
        }

        private Task IsNarrowViewChanged()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostingViewModel ViewModel
        {
            get { return (IPostingViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostingViewModel), typeof (PostingEntryView), new PropertyMetadata(null));

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => Shell.StyleManager;

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                var vm = tb.Tag as IPostingFieldViewModel<string>;
                if (vm != null)
                {
                    vm.UpdateValueInternal(tb.Text);
                }
            }
        }
    }
}
