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
            File.AppendAllText(@"%programdata%\LionsSoft\ExampleProcess.txt", "Avviato\r\n");
            Console.ReadKey();
        }
    }
}
