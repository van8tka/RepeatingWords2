using System;
using System.Diagnostics;
using NLog;
using RepeatingWords.iOS.LoggerService;
using Xamarin.Forms;

[assembly: Dependency(typeof(NLogLogger))]
namespace RepeatingWords.iOS.LoggerService
{
    public class NLogLogger : Interfaces.ILogger
    {
        private Logger log;

        public NLogLogger(Logger log)
        {
            this.log = log;
        }

        public void Debug(string message, params object[] args)
        {
            log.Debug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            log.Error(message, args);
        }

        public void Error(Exception e, string message, params object[] args)
        {
            log.Error(e, message, args);
        }

        public void Error(Exception e)
        {
            Debugger.Break();
            log.Error(e);
        }

        public void Fatal(string message, params object[] args)
        {
            log.Fatal(message, args);
        }

        public void Info(string message, params object[] args)
        {
            log.Info(message, args);
        }

        public void Trace(string message, params object[] args)
        {
            log.Trace(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            log.Warn(message, args);
        }
    }
}