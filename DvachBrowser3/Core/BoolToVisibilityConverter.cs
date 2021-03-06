﻿using System;
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

    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var invert = parameter == null || "true".Equals((string)parameter, StringComparison.OrdinalIgnoreCase);
            var v = !invert ? (bool)value : !(bool)value;
            return v ? 1.0 : 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (bool)value;
            return v ? "Да" : "Нет";
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

    public class EmptyStringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var invert = parameter == null || "true".Equals((string)parameter, StringComparison.OrdinalIgnoreCase);
            var s = value?.ToString();
            var se = string.IsNullOrWhiteSpace(s);
            var v = !invert ? !se : se;
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

    public class ListBulletConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "• " + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class NullableIntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ni = value as int?;
            if (ni == null)
            {
                return "Нет";
            }
            return ni.Value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new GridLength((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DoubleToNullableDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var nd = new double?((double) value);
            return nd;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var nd = (double?) value;
            return nd ?? 0.0;
        }
    }
}