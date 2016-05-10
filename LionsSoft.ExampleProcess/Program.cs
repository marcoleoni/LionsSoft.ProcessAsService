using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LionsSoft.ExampleProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ciao secco sono partito");
            File.AppendAllText(@"c:\programdata\LionsSoft\ExampleProcess.txt", "Avviato");
            Console.ReadKey();
        }
    }
}
