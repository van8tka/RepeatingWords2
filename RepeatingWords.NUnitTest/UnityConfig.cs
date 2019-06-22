using System;
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
            container.RegisterType<ILogger, TestLogger>();
            container.RegisterType<ILoggerService, Log>();
            container.Resolve<ILoggerService>();
            container.RegisterInstance(typeof(IUnitOfWork), new UnitOfWork(testPath.GetDatabasePath(string.Empty)));
            container.RegisterType<IInitDefaultDb, InitDefaultDb>();
            return container;
        }
    }

    internal class TestLogger : ILogger
    {
        public void Debug(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Error(string message, params object[] args)
        {
             Console.WriteLine(message, args);
        }

        public void Error(Exception e, string message, params object[] args)
        {
            Console.WriteLine(e);
            Console.WriteLine(message);
        }

        public void Error(Exception e)
        {
            Console.WriteLine(e);
        }

        public void Fatal(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Trace(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message, params object[] args)
        {
            Console.WriteLine(message);
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
