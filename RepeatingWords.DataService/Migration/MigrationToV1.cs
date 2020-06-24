using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net;
using RepeatingWords.DataService.Model;
using Microsoft.Data.Sqlite;

namespace RepeatingWords.DataService.Migration
{
    internal class MigrationToV1
    {
        private const int CURRENT_DB_VERSION = 1;
        private DbContext _context;

        public MigrationToV1(DbContext context)
        {
            _context = context;
        }

        public void ExecuteMigration()
        {
            if (!CheckVersion())
                MigrationProcess();
        }

        private void MigrationProcess()
        {
            try
            {
                Console.WriteLine("Begin migrate");
                _context.Database.BeginTransaction();
                //drop LastAction table
                _context.Database.ExecuteSqlCommand("drop table LastAction");
                //create Language table
                string create_lang_query = "CREATE TABLE IF NOT EXISTS Language (" +
                                           " Id INTEGER PRIMARY KEY, " +
                                           " NameLanguage TEXT, " +
                                           " PercentOfLearned INTEGER NOT NULL); ";
                _context.Database.ExecuteSqlCommand(create_lang_query);
                string create_def_language_query = "INSERT INTO Language (Id, NameLanguage, PercentOfLearned) " +
                                                   "VALUES (1, 'My collection' , 0) ;";
                _context.Database.ExecuteSqlCommand(create_def_language_query);
                //added columns to Dictionary table
                string added_column_to_dictionary = "ALTER TABLE Dictionary " +
                                                    "ADD IdLanguage INTEGER DEFAULT 0 NOT NULL; " +
                                                    "ALTER TABLE Dictionary " +
                                                    "ADD PercentOfLearned INTEGER DEFAULT 0 NOT NULL; " +
                                                    "ALTER TABLE Dictionary " +
                                                    "ADD IsBeginLearned INTEGER DEFAULT 0 NOT NULL; " +
                                                    "ALTER TABLE Dictionary " +
                                                    "ADD LastUpdated TEXT DEFAULT '1990-01-01 01:01:01.0000001' NOT NULL; ";
                _context.Database.ExecuteSqlCommand(added_column_to_dictionary);
                //update dictionaries to language - my collection
                string update_dictionary = "UPDATE Dictionary SET IdLanguage = 1 WHERE Id >0";
                _context.Database.ExecuteSqlCommand(update_dictionary);
                //added columns to Words
                string added_column_to_words = "ALTER TABLE Words ADD COLUMN IsLearned INTEGER DEFAULT 0 NOT NULL; ";
                _context.Database.ExecuteSqlCommand(added_column_to_words);
                //create version table
                string create_version =
                    "CREATE TABLE IF NOT EXISTS VersionDB (Id INTEGER PRIMARY KEY, VersionNumber INTEGER NOT NULL);";
                _context.Database.ExecuteSqlCommand(create_version);
                string insert_version = "INSERT INTO VersionDB (Id, VersionNumber) VALUES (1,1);";
                _context.Database.ExecuteSqlCommand(insert_version);
                _context.Database.CommitTransaction();
                Console.WriteLine("End migrate");
            }
            catch (Exception er)
            {
                _context.Database.RollbackTransaction();
                Console.WriteLine(er.Message);
            }
        }

        private bool CheckVersion()
        {
            try
            {
                SQLiteContext ctx = _context as SQLiteContext;
                int count = ctx.VersionDbs.Count();
                if (count == 0)
                {
                    VersionDB v = new VersionDB
                    {
                        Id = 0,
                        VersionNumber = CURRENT_DB_VERSION
                    };
                    ctx.VersionDbs.Add(v);
                    ctx.SaveChanges();
                }

                var version = ctx.VersionDbs.FirstOrDefault(x => x.VersionNumber == CURRENT_DB_VERSION);
                return version != null;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return false;
            }
        }
    }
}
