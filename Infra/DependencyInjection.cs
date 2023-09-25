using TempusFujit.ViewModels;

namespace TempusFujit.Infra
{
    public static class DependencyInjection
    {
        public static void RegisterRepositories(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();

            builder.Services.AddSingleton<LoginPage>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<Categories>();
            builder.Services.AddTransient<CategoriesVM>();

            builder.Services.AddSingleton<ClientOverview>();
            builder.Services.AddTransient<ClientOverviewVM>();

            builder.Services.AddSingleton<ClientTimesDisplay>();
            builder.Services.AddTransient<ClientTimesDisplayVM>();

        }
    }
}
