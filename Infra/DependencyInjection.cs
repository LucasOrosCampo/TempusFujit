using TempusFujit.ViewModels;

namespace TempusFujit.Infra
{
    public static class DependencyInjection
    {
        public static void RegisterRepositories(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddTransient<ClientTimesDisplayVM>();
            builder.Services.AddTransient<ClientOverviewVM>();
            builder.Services.AddSingleton<ClientOverview>();
            builder.Services.AddSingleton<ClientTimesDisplay>();

            builder.Services.AddSingleton<LoginPage>();
        }
    }
}
