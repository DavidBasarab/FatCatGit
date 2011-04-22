using System;
using System.Collections.Generic;
using System.IO;
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
                Console.WriteLine(arugment);
            }

            Console.WriteLine(Environment.CurrentDirectory);

            Console.Error.WriteLine("This is on the Error Stream");

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "random")
                {
                    string[] tempArgs = {
                                            @"F:\Test\Repo1", "51", "5242880"
                                        };

                    CreateRandomFiles(tempArgs);
                }

                var containWait = args.Where(i => i.ToLower() == "wait")
                                       .Count() > 0;

                if (containWait)
                {

                    Console.WriteLine("Going to sleep for 100 ms");

                    System.Threading.Thread.Sleep(50);

                    Console.WriteLine("Done sleeping for 100 ms");

                    Console.WriteLine("Going to sleep for 2500 ms");

                    System.Threading.Thread.Sleep(50);

                    Console.WriteLine("Done sleeping for 2500 ms");
                }
            }
        }

        private static void CreateRandomFiles(string[] args)
        {
            string filePath = string.Empty;
            var numberOfFilesToCreate = 750;
            var fileSizeInBytes = 24576;

            if (args.Length > 0)
            {
                filePath = args[0];

                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }
            }

            if (args.Length > 1)
            {
                numberOfFilesToCreate = int.Parse(args[1]);
            }

            if (args.Length > 2)
            {
                fileSizeInBytes = int.Parse(args[2]);
            }

            for (int fileNumber = 0; fileNumber < numberOfFilesToCreate; fileNumber++)
            {
                var fileText = new StringBuilder();

                for (int i = 0; i < fileSizeInBytes / 1024; i++)
                {
                    fileText.AppendLine(Extensions.GetRandomCharcterString(1024));
                }

                string fileFullName = string.Format("{0}{1}.txt", filePath, Extensions.GetRandomCharcterString(14));

                File.WriteAllText(fileFullName, fileText.ToString());

                Console.WriteLine("Done with file number {0}", fileNumber);
            }
        }
    }
}
