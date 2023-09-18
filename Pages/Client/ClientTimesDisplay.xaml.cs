using TempusFujit.Infra;
using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class ClientTimesDisplay : ContentPage
{
    ClientTimesDisplayVM vm;
    public ClientTimesDisplay()
    {
        vm = Services.Get<ClientTimesDisplayVM>() as ClientTimesDisplayVM;
        BindingContext = vm;
        InitializeComponent();
    }
}