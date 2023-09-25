using System.Globalization;
using TempusFujit.Infra;
using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class ClientOverview : ContentPage
{
    public ClientOverviewVM vm { get; set; }

    public ClientOverview()
    {
        vm = Services.Get<ClientOverviewVM>() as ClientOverviewVM;
        BindingContext = vm;
        this.Resources.Add("hoursConverter", new ConvertDecimalToRounded());
        InitializeComponent();
        NavigatedTo += ClientOverview_NavigatedTo;
    }

    private void ClientOverview_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        vm.ComputeHours();
        vm.loadCategories();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(ClientTimesDisplay)}?clientId={vm.ClientId}");
    }
}

public class ConvertDecimalToRounded : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double dValue = (double)value;
        return (int)dValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}