using Microsoft.EntityFrameworkCore;
using RepeatingWords.DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepeatingWords.DataService
{
   public class SQLiteContext:DbContext, IDbContext
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
        public DbSet<Model.LastAction> LastAction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

    }
}
