using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TempusFujit.Infra
{
    public interface IDbContextFactory
    {
        public DatabaseContext CreateDbContext();
    }
    public class DbContextFactory : IDbContextFactory
    {
        private IConfiguration _configuration;
        public DbContextFactory(IConfiguration configuration) {
            _configuration = configuration;
        }

        public DatabaseContext CreateDbContext ()
        {
            var settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlite(settings.DbConnectionString);
            return new DatabaseContext(builder.Options);
        }
    }
}
