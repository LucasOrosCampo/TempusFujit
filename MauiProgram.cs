using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TempusFujit.Infra;

namespace TempusFujit;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("YsabeauOffice-Black.ttf", "YsabeauOfficeBlack");
            });
        builder.Configuration.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
        var settings = builder.Configuration.GetRequiredSection("Settings").Get<Settings>();

        DbPathManager.InitializeDbConfig(settings.DbConnectionString);
        var dbConnectionString = DbPathManager.GetDbConnectionString();
        DatabaseContextFactory.SetDbConnectionString(dbConnectionString);
        builder.Services.AddDbContextFactory<DatabaseContext, DatabaseContextFactory>();

        DependencyInjection.RegisterRepositories(builder);
        var app = builder.Build();

        var dbFactory = app.Services.GetRequiredService(typeof(IDbContextFactory<DatabaseContext>)) as IDbContextFactory<DatabaseContext>;
        using var db = dbFactory.CreateDbContext();
        db.Database.Migrate();

        return app;
    }
}
