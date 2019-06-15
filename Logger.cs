using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Logger
    {

        public static void LogErrorAndQuit(string m)
        {
            Console.WriteLine(m);
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void LogError(string m)
        {
            Console.WriteLine(m);
            Console.ReadKey();
        }

    }
}
