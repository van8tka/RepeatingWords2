using RepeatingWords.Interfaces;
using System;

namespace RepeatingWords.LoggerService
{
    public class Log:ILoggerService
    {
        public static ILogger Logger;
        public Log(ILogger log)
        {
            Logger = log ?? throw new ArgumentNullException(nameof(log));
        }
    }
}
