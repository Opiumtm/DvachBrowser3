using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DvachBrowser3
{
    /// <summary>
    /// Конвертер bool в Visibility.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var invert = parameter == null || "true".Equals((string)parameter, StringComparison.OrdinalIgnoreCase);
            var v = !invert ? (bool)value : !(bool)value;
            return v ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (bool)value;
            return !v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var v = (bool)value;
            return !v;
        }
    }


    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var invert = parameter == null || "true".Equals((string)parameter, StringComparison.OrdinalIgnoreCase);
            var v = !invert ? value != null : value == null;
            return v ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? r;
            r = (bool)value;
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool?)value ?? false;
        }
    }
}