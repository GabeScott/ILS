using System;
using System.Collections.Generic;
using System.IO;

namespace ILS
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string ILSFilename = "C:/Users/Gabe Scott/Desktop/ILS.ils";

            File file = new File(ILSFilename);

            try
            {
                file.LoadInputAndExecute();
            }catch(ILSException e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadKey();

        }
    }
}
