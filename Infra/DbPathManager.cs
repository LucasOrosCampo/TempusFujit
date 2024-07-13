using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempusFujit.Infra
{
    public static class DbPathManager
    {
        const string configFileName = "database.config";
        static string DatabaseConfigPath => Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName));
        static string DbConnectionString(string dbPath) => $"Data Source={dbPath};";


        public static string? GetDbConnectionString()
        {
            if (File.Exists(DatabaseConfigPath))
            {
                using var reader = new StreamReader(DatabaseConfigPath);
                var persistedDbConnectionString = reader.ReadLine();
                return persistedDbConnectionString;
            }
            return null;
        }
        public static void InitializeDbConfig(string defaultDbConnectionString)
        {
            if (File.Exists(DatabaseConfigPath)) return; 
            using var newStream = File.Create(DatabaseConfigPath);
            using var writer = new StreamWriter(newStream);
            writer.WriteLine(defaultDbConnectionString);
        }

        public static bool UpdateDbPath(string newDirectoryContainingDbPath)
        {
            if (!Directory.Exists(newDirectoryContainingDbPath)) return false;
            var newDbPath = Path.Combine(newDirectoryContainingDbPath, "TempusFujit.db");
            var newdBConnectionString = DbConnectionString(newDbPath);
            if (File.Exists(DatabaseConfigPath))
            {
                using var writer = new StreamWriter(DatabaseConfigPath, false);
                writer.WriteLine(newdBConnectionString);
            }
            else
            {
                using var newStream = File.Create(DatabaseConfigPath);
                using var writer = new StreamWriter(newStream);
                writer.WriteLine(newdBConnectionString);
            }


            DatabaseContextFactory.SetDbConnectionString(newdBConnectionString);
            InitializeDatabaseIfNew(newDbPath);
            return true;
        }

        static void InitializeDatabaseIfNew(string databasePath)
        {
            if (!File.Exists(databasePath))
            {
                using var db = Services.DbFactory.CreateDbContext();
                db.Database.Migrate();
            }
        }
    }
}
