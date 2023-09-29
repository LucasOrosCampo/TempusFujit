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
        NavigatedTo += ClientTimesDisplay_NavigatedTo;
    }

    private void ClientTimesDisplay_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        vm.loadAllCategories();
    }
}

