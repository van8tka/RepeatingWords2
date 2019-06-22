using System;
using System.IO;
using System.Runtime.CompilerServices;
using NLog.Config;
using NLog.Targets;
using RepeatingWords.Droid.LoggerService;
using RepeatingWords.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(NLogManager))]
namespace RepeatingWords.Droid.LoggerService
{
    public class NLogManager : ILogManager
    {
        private string LOG_FILE_NAME = "log_cardsofwords.txt";

        public NLogManager()
        {
            var config = new LoggingConfiguration();
            ConsoleConfigurationLogger(config);
            FileConfigurationLogger(config);
            NLog.LogManager.Configuration = config;
        }

        private void FileConfigurationLogger(LoggingConfiguration config)
        {
            var fileTarget = new FileTarget();
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileTarget.FileName = Path.Combine(folder, LOG_FILE_NAME);
            config.AddTarget("file", fileTarget);
            var fileRule = new LoggingRule("*", NLog.LogLevel.Warn, fileTarget);
            config.LoggingRules.Add(fileRule);
        }

        private void ConsoleConfigurationLogger(LoggingConfiguration config)
        {
            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);
            var consoleRule = new LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(consoleRule);
        }

        public ILogger GetLog([CallerFilePath] string callerFilePath = "")
        {
            string filename = callerFilePath;
            if(filename.Contains("/"))             
                filename = Path.GetFileName(filename);
            var logger = NLog.LogManager.GetLogger(filename);
            return new NLogLogger(logger);
        }

       

    }
}