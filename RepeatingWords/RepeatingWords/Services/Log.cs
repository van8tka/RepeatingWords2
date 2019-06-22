using RepeatingWords.Interfaces;
using System;

namespace RepeatingWords.Services
{
    internal class Log:ILoggerService
    {
        internal static ILogger Logger;
        internal Log(ILogger log)
        {
            Logger = log ?? throw new ArgumentNullException(nameof(log));
        }
    }
}
