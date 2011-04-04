using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLineUnitTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is echo base.");

            foreach (var arugment in args)
            {
                Console.Write(arugment);
            }

            Console.Write(Environment.CurrentDirectory);

            Console.Error.WriteLine("This is on the Error Stream");
        }
    }
}
