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

    }
}
