using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235
using DvachBrowser3.ViewModels;

namespace DvachBrowser.Views
{
    public sealed class ViewModeContainer : ContentControl
    {
        public ViewModeContainer()
        {
            this.DefaultStyleKey = typeof(ViewModeContainer);
        }

        public PostViewMode ViewMode
        {
            get { return (PostViewMode) GetValue(ViewModeProperty); }
            set { SetValue(ViewModeProperty, value); }
        }

        public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register("ViewMode", typeof (PostViewMode), typeof (ViewModeContainer),
            new PropertyMetadata(PostViewMode.Show));
    }
}
