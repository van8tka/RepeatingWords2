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
        public string GetDatabasePath(string filename)
        {
            Log.Logger.Info("Get path to db");
            // string newPathDb = MainActivity.Instance.GetDatabasePath(filename).AbsolutePath;
            string newPathDb = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            newPathDb = Path.Combine(newPathDb, filename);
            return newPathDb;
          
        }
    }
 }