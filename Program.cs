using System;
using System.Collections.Generic;
using System.IO;

namespace ILS
{
    class Program
    {
       
        static void Main(string[] args)
        {
            File file = new File("C:\\Users\\Gabe Scott\\source\\repos\\ILS\\ILS.ils");
            file.ReadInput();
            file.Interpret();

            Console.ReadKey();

        }
    }
}
