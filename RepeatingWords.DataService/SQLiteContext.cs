using Microsoft.EntityFrameworkCore;
using System;
using RepeatingWords.DataService.Model;

namespace RepeatingWords.DataService
{
    public class SQLiteContext:DbContext 
    {
        //ctor
        public SQLiteContext(string dbpath)
        {
            _dbpath = dbpath ?? throw new ArgumentNullException(nameof(dbpath));
             Database.Migrate();          
        }

        private readonly string _dbpath;
        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<Words> Words { get; set; }
        public DbSet<LastAction> LastActions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_dbpath.Contains("memory"))           
                optionsBuilder.UseSqlite(@"Data Source =:memory:");          
            else
                optionsBuilder.UseSqlite($"Filename={_dbpath}");
        }

    }
}
