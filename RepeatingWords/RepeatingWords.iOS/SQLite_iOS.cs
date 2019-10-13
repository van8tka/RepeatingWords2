using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using RepeatingWords.Interfaces;
using RepeatingWords.iOS;
using UIKit;
using Xamarin.Forms;

[assembly:Dependency(typeof(SQLite_iOS))]
namespace RepeatingWords.iOS
{
   public class SQLite_iOS:ISQLite
    {
        public string GetDatabasePath(string filename)
        {
            throw new NotImplementedException();
        }
    }
}