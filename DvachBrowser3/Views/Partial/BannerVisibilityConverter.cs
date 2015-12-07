using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Views.Partial
{
    public class BannerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var b = (PageBannerBehavior)value;
                switch (b)
                {
                    case PageBannerBehavior.Enabled:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}