using System;
using System.Diagnostics;
using System.IO;
using RepeatingWords.Droid;
using Xamarin.Forms;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;

[assembly: Dependency(typeof(SQLite_Android))]
namespace RepeatingWords.Droid
{
    public class SQLite_Android : ISQLite
    {
        private string oldDbName = "repeatwords.db";
        public string GetDatabasePath(string filename)
        {
            Log.Logger.Info("Get path to db");
            string newPathDb = MainActivity.Instance.GetDatabasePath(filename).AbsolutePath;
            try
            {
                Log.Logger.Info("Standart path to db: " + newPathDb);
                string olderPathDb = null;
                if (IsDbInOtherPath(oldDbName, ref olderPathDb))
                {
                    Log.Logger.Info("Old path to db is EXIST ");
                    CopyToStandartAndroidDbPath(newPathDb, olderPathDb);
                }
                else
                    Log.Logger.Info("Old path to db is NOT EXIST ");
                return newPathDb;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                return newPathDb;
            }
        }

        private void CopyToStandartAndroidDbPath(string newPath, string olderPath)
        {
            try
            {
                Log.Logger.Info("Copy db from older path to new path");
                File.Move(olderPath, newPath);
                Log.Logger.Info("Remove older db if exist");
                if(File.Exists(olderPath))
                    File.Delete(olderPath);
            }
            catch (Exception er)
            {
                Log.Logger.Error("Error move and delete db from older path to newpath" + er.Message);
                throw;
            }
        }

        private bool IsDbInOtherPath(string filename,ref string olderPathDb)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string path = Path.Combine(documentsPath, filename);
                Log.Logger.Info("Old path to db: " + path);
                olderPathDb = path;
                return File.Exists(path);
            }
            catch (Exception er)
            {
                Log.Logger.Error("Error get old path to DB"+er.Message);
                throw;
            }
        }
    }
 }