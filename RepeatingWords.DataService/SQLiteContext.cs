using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net;
using RepeatingWords.DataService.Model;
using Microsoft.Data.Sqlite;
using RepeatingWords.DataService.Migration;


namespace RepeatingWords.DataService
{
    public sealed class SQLiteContext:DbContext 
    {
       //ctor
        public SQLiteContext(string dbpath)
        {
            _dbpath = dbpath ?? throw new ArgumentNullException(nameof(dbpath));
            if (!File.Exists(dbpath))
                Database.EnsureCreated();
            CheckMigration();
        }

        private void CheckMigration()
        {
            var migration = new MigrationToV1(this);
            migration.ExecuteMigration();
        }

        private readonly string _dbpath;
        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<Words> Words { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<VersionDB> VersionDbs { get; set; }

         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_dbpath.Contains("memory"))
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                optionsBuilder.UseSqlite(connection);
            }                      
            else
                optionsBuilder.UseSqlite($"Filename={_dbpath}");
        }


    }
}
