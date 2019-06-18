using Microsoft.EntityFrameworkCore;

namespace RepeatingWords.DataService
{
    public class SQLiteContext:DbContext 
    {
        //ctor
        public SQLiteContext(string dbpath)
        {        
            DbPath = dbpath;
            Database.Migrate();
        }

        public string DbPath { get; }
        public DbSet<Model.Dictionary> Dictionaries { get; set; }
        public DbSet<Model.Words> Words { get; set; }
        public DbSet<Model.LastAction> LastActions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

    }
}
