﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using NLog.Config;
using NLog.Targets;
using RepeatingWords.iOS.LoggerService;
using RepeatingWords.Interfaces;


[assembly: Xamarin.Forms.Dependency(typeof(NLogManager))]
namespace RepeatingWords.iOS.LoggerService
{
    public class NLogManager : ILogManager
    {
        private string LOG_FILE_NAME = "_log_cardsofwords.txt";

        public NLogManager()
        {
            var config = new LoggingConfiguration();
            ConsoleConfigurationLogger(config);
            FileConfigurationLogger(config);
#if RELEASE
                    MailConfigurationLogger(config);
#endif
            NLog.LogManager.Configuration = config;
        }

        private void MailConfigurationLogger(LoggingConfiguration config)
        {
            var mailTarget = new NLog.MailKit.MailTarget();
            mailTarget.Name = "mail_cardsofwords_log";
            mailTarget.Subject = "CARDSOFWORDS_ERROR_LOG";
            mailTarget.To = "ioan.kuzmuk@gmail.com";
            mailTarget.From = "van8tka@mail.ru";
            mailTarget.SmtpUserName = "van8tka@mail.ru";
            mailTarget.SmtpPassword = "dfyznrf6734004";
            mailTarget.SmtpServer = "smtp.mail.ru";
            mailTarget.SecureSocketOption = MailKit.Security.SecureSocketOptions.Auto;
            mailTarget.SmtpPort = 465;
            mailTarget.SmtpAuthentication = NLog.MailKit.SmtpAuthenticationMode.Basic;
            config.AddTarget("mail", mailTarget);
            var mailRule = new LoggingRule("*", NLog.LogLevel.Error, mailTarget);
            config.LoggingRules.Add(mailRule);
        }

        private void FileConfigurationLogger(LoggingConfiguration config)
        {
            var fileTarget = new FileTarget();
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dirLogPath = Path.Combine(folder, "CardsOfWordsLogs");
            if (!Directory.Exists(dirLogPath))
                Directory.CreateDirectory(dirLogPath);
            string filePathLog = Path.Combine(dirLogPath, DateTime.Now.ToString("dd_MM_yyyy") + LOG_FILE_NAME);
            fileTarget.FileName = filePathLog;
            if (!File.Exists(filePathLog))
                File.Create(filePathLog);
            config.AddTarget("file", fileTarget);
            LoggingRule rule;
#if RELEASE
        rule = new LoggingRule("*", NLog.LogLevel.Error, fileTarget);     
#else
            rule = new LoggingRule("*", NLog.LogLevel.Info, fileTarget);
#endif
            config.LoggingRules.Add(rule);
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
            if (filename.Contains("/"))
                filename = Path.GetFileName(filename);
            var logger = NLog.LogManager.GetLogger(filename);
            return new NLogLogger(logger);
        }
    }
}