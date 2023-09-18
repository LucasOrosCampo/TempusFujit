using TempusFujit.Infra;
using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class ClientOverview : ContentPage
{
    ClientOverviewVM vm;

    public ClientOverview()
    {
        vm = Services.Get<ClientOverviewVM>() as ClientOverviewVM;
        BindingContext = Services.Get<ClientOverviewVM>() as ClientOverviewVM;
        InitializeComponent();
    }
}