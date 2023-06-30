using Microsoft.Maui.Controls;

namespace TempusFujit;

public partial class LoginPage : ContentPage
{
	int count = 0;

	public LoginPage()
	{
		InitializeComponent();
	}

	private void OnLogInClicked(object sender, EventArgs e)
	{
		if (passwordEntry.Text == "123")
		{
			Shell.Current.GoToAsync("//mainPage");
		}
			
	}
}

