namespace TempusFujit;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute("Client/Overview", typeof(ClientOverview));
        Routing.RegisterRoute("Client/TimesDisplay", typeof(ClientTimesDisplay));
    }
}
