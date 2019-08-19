using System;

namespace ILS
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string ILSFilename = "ILS.ils";

            File file = new File(ILSFilename);
            try
            {
                file.LoadAllLinesAndExecute();
            }catch(ILSException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

        }
    }
}
