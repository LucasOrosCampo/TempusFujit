using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Windows.UI;

namespace TempusFujit.Infra
{

    public class DatabaseContextFactory : IDbContextFactory<DatabaseContext>
    {
        static string dbConnectionString;

        public static void SetDbConnectionString(string newDbConnectionString)
        {
            dbConnectionString = newDbConnectionString;
        }

        public DatabaseContext CreateDbContext()
        {
            var optBuilder = new DbContextOptionsBuilder<DatabaseContext>().UseSqlite(dbConnectionString);
            return new DatabaseContext(optBuilder.Options);
        }
    }
}
