using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TempusFujit.Infra;
using System.Reflection;

namespace TempusFujit.Database
{
    public class DesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("TempusFujit.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            var settings = config.GetRequiredSection("Settings").Get<Settings>();

            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlite(settings.DbConnectionString);
            return new DatabaseContext(builder.Options);
        }

    }
}

    

