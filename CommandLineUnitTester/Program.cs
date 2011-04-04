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
                Console.Write(arugment);
            }

            Console.Write(Environment.CurrentDirectory);

            Console.Error.WriteLine("This is on the Error Stream");

            if (args != null && args.Length > 0)
            {
                if (args[0].ToLower() == "random")
                {
                    string[] tempArgs = {
                                            @"C:\Test\Repo1", "100", "5242880"
                                        };

                    CreateRandomFiles(tempArgs);
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
