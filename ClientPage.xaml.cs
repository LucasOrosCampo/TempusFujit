using TempusFujit.ViewModels;

namespace TempusFujit;

public partial class ClientPage : ContentPage
{
    ClientPageViewModel vm = (ClientPageViewModel)MauiWinUIApplication.Current.Services.GetRequiredService(typeof(ClientPageViewModel));
    
	public ClientPage()
	{
        InitializeComponent();
    }

    void ShowTrashIcon(object sender, PointerEventArgs args)
    {
        var btn = (ImageButton)((Grid)sender).Children[2];
        btn.IsVisible = true;
    }
    void HideTrashIcon(object sender, PointerEventArgs args)
    {
        var btn = (ImageButton)((Grid)sender).Children[2];
        btn.IsVisible = false;
    }
}