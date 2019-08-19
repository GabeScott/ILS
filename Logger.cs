using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Logger
    {

        public static void LogErrorAndQuit(string errorMessage)
        {
            Console.WriteLine("Exception thrown on line " + LinePointer.GetCurrentLine() + ": " + errorMessage);
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void FoundEndOfFile()
        {
            Console.WriteLine("\nEnd of file token found. Exiting.");
            Console.ReadKey();
            Environment.Exit(0);
        }

    }
}
