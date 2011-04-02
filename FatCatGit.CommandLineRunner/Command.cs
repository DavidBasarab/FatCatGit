using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatGit.CommandLineRunner
{
    public class Command
    {
        public Command(string commandFullLocation)
        {
            CommandFullLocation = commandFullLocation;
        }

        public Command(string commandFullLocation, string arguments)
        {
            CommandFullLocation = commandFullLocation;
            Arguments = arguments;
        }

        public string CommandFullLocation { get; set; }

        public string Arguments { get; set; }
    }
}
