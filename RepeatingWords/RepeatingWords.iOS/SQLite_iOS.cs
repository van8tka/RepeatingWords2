using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using RepeatingWords.Interfaces;
using RepeatingWords.iOS;
using UIKit;
using Xamarin.Forms;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System.IO;
[assembly:Dependency(typeof(SQLite_iOS))]
namespace RepeatingWords.iOS
{
   public class SQLite_iOS:ISQLite
    {
        public string GetDatabasePath(string filename)
        {
            Log.Logger.Info("Get path to db");
           return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"..","Library", filename);
        }
    }
}