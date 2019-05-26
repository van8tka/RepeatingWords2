using System;
using RepeatingWords.Droid;
using System.IO;
using Xamarin.Forms;
using RepeatingWords.Interfaces;

[assembly: Dependency(typeof(SQLite_Android))]
namespace RepeatingWords.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public string GetDatabasePath(string filename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);                    
            return Path.Combine(documentsPath, filename); ;
        }
    }
 }