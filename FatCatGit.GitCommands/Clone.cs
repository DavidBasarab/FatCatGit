using System;
using FatCatGit.GitCommands.Args;

namespace FatCatGit.GitCommands
{
    public class Clone : BaseCommand
    {
        // git.exe clone -v --progress "C:/Test/Repo1" "C:/Test/Test2"

        public Clone(string projectLocation) : base(projectLocation)
        {
        }

        public string RepositoryToClone { get; set; }

        public string Destination { get; set; }

        protected override string GitCommandString
        {
            get { return string.Format("clone -v --verbose --progress \"{0}\" \"{1}\"", RepositoryToClone, Destination); }
        }

        
    }
}