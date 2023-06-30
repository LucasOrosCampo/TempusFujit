using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class ClientPage : ContentPage
{
    ClientPageViewModel vm = (ClientPageViewModel)MauiWinUIApplication.Current.Services.GetRequiredService(typeof(ClientPageViewModel));
    
	public ClientPage()
	{
        InitializeComponent();
    }

	private void OnBack(object sender, EventArgs e)
	{
        Shell.Current.GoToAsync("//mainPage");
    }


}