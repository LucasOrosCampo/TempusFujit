using System.Globalization;

namespace TempusFujit.Converters
{
    public class StringNotEmptyThenEnabledConverter : IValueConverter
    {
        public static StringNotEmptyThenEnabledConverter Instance { get; } = new StringNotEmptyThenEnabledConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = (string)value;
            return !string.IsNullOrEmpty(stringValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
