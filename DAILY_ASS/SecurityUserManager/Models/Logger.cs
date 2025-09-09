using System;
using System.IO;

namespace SecureUserManager.Models
{
    public static class Logger
    {
        private static readonly string LogFilePath = "app.log";

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(LogFilePath, $"{DateTime.UtcNow}: {message}\n");
            }
            catch { /* Ignore logging errors */ }
        }

        public static void LogError(string error)
        {
            Log($"ERROR: {error}");
        }
    }
}