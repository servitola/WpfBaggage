using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfBaggage.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((parameter is string && parameter as string == "invert")
                        ? !(bool)value
                        : (bool)value)
                       ? Visibility.Visible
                       : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter is string && parameter as string == "invert"
                       ? (Visibility)value != Visibility.Visible
                       : (Visibility)value == Visibility.Visible;
        }
    }
}
