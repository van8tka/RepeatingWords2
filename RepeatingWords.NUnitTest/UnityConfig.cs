using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Repositories;
using RepeatingWords.Interfaces;
using RepeatingWords.Services;
using Unity;

namespace RepeatingWords.NUnitTest
{
    internal class UnityConfig
    {
        internal static UnityContainer Load()
        {
            var container = new UnityContainer();
            var testPath = new TestSQLitePath();
            container.RegisterInstance(typeof(ISQLite), testPath);
            container.RegisterInstance(typeof(IUnitOfWork), new UnitOfWork(testPath.GetDatabasePath(string.Empty)));
            container.RegisterType<IInitDefaultDb, InitDefaultDb>();
            return container;
        }
    }


    internal class TestSQLitePath : ISQLite
    {
        //для тестирования sqlite in memory
        public string GetDatabasePath(string filename)
        {
            return ":memory:";         
        }
    }
}
