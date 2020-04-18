using RepeatingWords.Droid;
using Xamarin.Forms;
using RepeatingWords.Interfaces;

[assembly: Dependency(typeof(SQLite_Android))]
namespace RepeatingWords.Droid
{
    public class SQLite_Android : ISQLite
    {
        public string GetDatabasePath(string filename)
        {
            return MainActivity.Instance.GetDatabasePath(filename).AbsolutePath;
        }
    }
 }