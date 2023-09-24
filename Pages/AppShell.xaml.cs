namespace TempusFujit;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ClientOverview), typeof(ClientOverview));
        Routing.RegisterRoute(nameof(ClientTimesDisplay), typeof(ClientTimesDisplay));
    }
}
