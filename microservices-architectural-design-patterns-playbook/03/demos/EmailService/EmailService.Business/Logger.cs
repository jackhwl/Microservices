using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Business
{
    public static class Logger
    {
        public static void LogInfo(string log)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(log);
            Console.ResetColor();
        }

        public static void LogDebug(string log)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(log);
            Console.ResetColor();
        }

        public static void LogError(Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {error.Message}");
            Console.ResetColor();
        }
    }
}
