using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Logger
    {

        public static void LogErrorAndQuit(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void LogError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.ReadKey();
        }

        public static void FoundEndOfFile()
        {
            Console.WriteLine();
            Console.WriteLine("End of file token found. Exiting.");
            Console.ReadKey();
            Environment.Exit(0);

        }

    }
}
