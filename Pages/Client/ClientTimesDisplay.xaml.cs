using System.Globalization;
using TempusFujit.Infra;
using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class ClientTimesDisplay : ContentPage
{
    public ClientTimesDisplayVM vm { get; set; }
    public ClientTimesDisplay()
    {
        vm = Services.Get<ClientTimesDisplayVM>() as ClientTimesDisplayVM;
        BindingContext = vm;
        InitializeComponent();
        Resources.Add("TimespanHourMinConverter", new TimespanHourMinConverter());
        Resources.Add("hoursConverter", new ConvertDecimalToRounded());

        NavigatedTo += ClientTimesDisplay_NavigatedTo;
    }

    private void ClientTimesDisplay_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        vm.loadAllCategories();
        vm.FilterAndCompute();
    }
}

public class TimespanHourMinConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var span = (TimeSpan)value;
        return $"{span.Hours}:{span.Minutes:00}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}