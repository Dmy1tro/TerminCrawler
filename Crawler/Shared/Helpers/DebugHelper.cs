using System;

namespace Shared.Helpers
{
    public class DebugHelper
    {
        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"{DateTime.Now} -> Error occured :(\n" +
                              $"Error message: {message}\n");

            SetDefaultColor();
        }

        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"{DateTime.Now} -> Info: {message}\n");

            SetDefaultColor();
        }

        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"{DateTime.Now} -> Success: {message}\n");

            SetDefaultColor();
        }

        private static void SetDefaultColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
