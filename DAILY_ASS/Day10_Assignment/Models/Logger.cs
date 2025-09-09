using System;
using System.IO;

namespace Day10_Assignment.Models
{
    public class Logger
    {
        private static readonly string logFile = Path.Combine("Logs", "log.txt");

        public static void Log(string message)
        {
            Directory.CreateDirectory("Logs");
            File.AppendAllText(logFile, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

    }
}
