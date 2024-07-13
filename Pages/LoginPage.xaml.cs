namespace TempusFujit;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnLogInClicked(object sender, EventArgs e)
    {
        if (passwordEntry.Text == "123")
        {
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}

