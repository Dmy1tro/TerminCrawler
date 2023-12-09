using System;

namespace Crawler.Services.Helpers
{
    public class DebugHelper
    {
        public static void LogError(Exception ex)
        {
            Write($"{DateTime.Now} -> Error: {ex}\n", ConsoleColor.Red);
        }

        public static void LogError(string message)
        {
            Write($"{DateTime.Now} -> Error: {message}\n", ConsoleColor.Red);
        }

        public static void LogInfo(string message)
        {
            Write($"{DateTime.Now} -> Info: {message}\n", ConsoleColor.White);
        }

        public static void LogSuccess(string message)
        {
            Write($"{DateTime.Now} -> Success: {message}\n", ConsoleColor.Green);
        }

        private static void Write(string message, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
        }
    }
}
