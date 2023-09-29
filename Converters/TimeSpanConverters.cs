using System.Globalization;

namespace TempusFujit.Converters
{
    public class Timespan2TotalHourAndMinutes : IValueConverter
    {
        public static Timespan2TotalHourAndMinutes Instance { get; } = new Timespan2TotalHourAndMinutes();


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan spanValue = (TimeSpan)value;
            var hours = (int)spanValue.TotalHours;
            var minutes = spanValue.Minutes;
            var displayY = hours > 0 && minutes > 0;
            var separator = displayY ? " y " : " ";
            return $"{GetString("hora", hours)} {separator} {GetString("minuto", minutes)}";
            string GetString(string type, int value) => value == 0
                                                        ? ""
                                                        : value == 1
                                                            ? value.ToString() + " " + type
                                                            : value.ToString() + " " + type + "s";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Timespan2TimeOfDay : IValueConverter
    {
        public static Timespan2TimeOfDay Instance { get; } = new Timespan2TimeOfDay();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan spanValue = (TimeSpan)value;
            return $"{spanValue.Hours:00}:{spanValue.Minutes:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IsNotEmptyTimespan : IValueConverter
    {
        public static IsNotEmptyTimespan Instance { get; set; } = new IsNotEmptyTimespan();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ts = (TimeSpan)value;
            return ts.Hours != 0 || ts.Minutes != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
