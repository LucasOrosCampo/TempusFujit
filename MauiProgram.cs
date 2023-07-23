using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TempusFujit.Infra;
using CommunityToolkit.Maui;

namespace TempusFujit;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("TempusFujit.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

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
        
        builder.Configuration.AddConfiguration(config);

        var settings = builder.Configuration.GetRequiredSection("Settings").Get<Settings>();

        builder.Services.AddDbContextFactory<DatabaseContext>(options => options.UseSqlite(settings.DbConnectionString));

        var dbBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        using var dbContext = new DatabaseContext(dbBuilder.UseSqlite(settings.DbConnectionString).Options);
        dbContext.Database.Migrate();

        builder.Services.AddTransient<MainPage>();
        DependencyInjection.RegisterRepositories(ref builder);

        return builder.Build();
    }



}
