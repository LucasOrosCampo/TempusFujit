using Microsoft.EntityFrameworkCore;
using TempusFujit.Models;

namespace TempusFujit.Infra
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
