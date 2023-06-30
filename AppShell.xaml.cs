namespace TempusFujit;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("mainPage", typeof(MainPage));
        Routing.RegisterRoute("ClientPage", typeof(ClientPage));
	}
}
