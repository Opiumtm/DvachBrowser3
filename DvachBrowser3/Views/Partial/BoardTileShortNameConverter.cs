using System;
using Windows.UI.Xaml.Data;

namespace DvachBrowser3.Views.Partial
{
    public class BoardTileShortNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"/{value}/";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}