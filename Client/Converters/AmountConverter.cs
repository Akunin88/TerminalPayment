using System.Globalization;
using System.Windows.Data;

namespace Client.Converters
{
    public class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value == null || !int.TryParse(value.ToString(), out int val))
                return false;
            return val > 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
            => throw new NotImplementedException();
    }
}
