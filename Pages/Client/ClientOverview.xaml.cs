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
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(ClientTimesDisplay)}?clientId={vm.ClientId}");
    }
}