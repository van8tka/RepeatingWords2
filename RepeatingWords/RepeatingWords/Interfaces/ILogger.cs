using System;

namespace RepeatingWords.Interfaces
{
    public interface ILogger
    {
        void Trace(string mesage, params object[] args);
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, params object[] args);
        void Error(Exception e, string message, params object[] args);
        void Error(Exception e);
        void Fatal(string message, params object[] args);
    }
}
