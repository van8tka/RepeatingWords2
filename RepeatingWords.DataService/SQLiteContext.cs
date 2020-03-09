using Microsoft.EntityFrameworkCore;
using System;
using RepeatingWords.DataService.Model;
using Microsoft.Data.Sqlite;

namespace RepeatingWords.DataService
{
    public sealed class SQLiteContext:DbContext 
    {
        //ctor
        public SQLiteContext(string dbpath)
        {
            _dbpath = dbpath ?? throw new ArgumentNullException(nameof(dbpath));
            // Database.Migrate();      
            //first start app using  Database.EnsureCreated(), 
            //then you must change to Database.Migrate() to using migration 
            Database.EnsureCreated();
        }

       
        private readonly string _dbpath;
        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<Words> Words { get; set; }
        public DbSet<Language> Languages { get; set; }


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
