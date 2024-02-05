using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Client.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value == null || !int.TryParse(value.ToString(), out int val))
                return 0;
            return val;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
            => value == null ? null : value.ToString();
    }
}
