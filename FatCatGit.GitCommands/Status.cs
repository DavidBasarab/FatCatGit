using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatGit.GitCommands
{
    public class Status : Command
    {
        public Status(string projectLocation)
            :base(projectLocation)
        {
            
        }

        protected override string GitCommandString
        {
            get { return "status"; }
        }
    }
}
